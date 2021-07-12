using MusicKG.Scheduler.Engine;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Job;
using MusicKG.Scheduler.Service;
using MusicKG.Scheduler.Service.MongoDB;
using MusicKG.Scheduler.Service.Settings;
using MusicKG.Scheduler.Executor;
using MusicKG.Scheduler.Executor.Settings;
using MusicKG.Scheduler.Engine.Settings;
using MusicKG.Scheduler.DataAccess;
using MusicKG.Scheduler.DataAccess.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;
using System;
using System.Linq;

namespace MusicKG.Scheduler.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRunner(this IServiceCollection serivces)
        {
            return serivces.AddHostedService<JobWorker>();
        }

        public static IServiceCollection AddScheduler(this IServiceCollection service, IConfiguration configuration, Func<IServiceCollection, IConfiguration, IServiceCollection> customerActionRegister)
        {
            service = service.AddConfiguration<JobSettings>(configuration)
                .AddConfiguration<SchedulerDBSettings>(configuration)
                .AddConfiguration<JobTaskSettings>(configuration)
                .AddConfiguration<LockSettings>(configuration)
                .AddConfiguration<SchedulerSettings>(configuration)
                .AddConfiguration<TaskHistorySettings>(configuration);

            service = service.AddSingleton<ISchedulerDbContext, SchedulerDbContext>()
                .AddSingleton<IJobService, MongoDBJobService>()
                .AddScoped<IJob, ScheduledJob>()
                .AddScoped<IJobFactory, ScheduledJobFactory>()
                .AddScoped<ISchedulerFactory, StdSchedulerFactory>()
                .AddScoped<IJobTaskService, MongoDBJobTaskService>()
                .AddScoped<ILocker, MongoDBLocker>()
                .AddScoped<IJobEngine, JobEngine>()
                .AddScoped<IJobTaskExecutor, JobTaskExecutor>()
                .AddScoped<Func<string, IJobAction>>(provider => actionType =>
                {
                    var actions = provider.GetServices<IJobAction>();

                    var instance = actions?.FirstOrDefault(action => action.ActionType == actionType);

                    return instance;
                })
                .AddScoped<Func<string, IActionExecutor>>(provider => executorType =>
                {
                    var actions = provider.GetServices<IActionExecutor>();

                    var instance = actions?.FirstOrDefault(action => action.ExecutorType == executorType);

                    return instance;
                });
                
            return customerActionRegister == null ? service : customerActionRegister(service, configuration);
        }

        public static IServiceCollection AddSchedulerManagement(this IServiceCollection service, IConfiguration configuration)
        {
            service = service.AddConfiguration<SchedulerDBSettings>(configuration.GetSection(nameof(SchedulerDBSettings)))
                .AddConfiguration<LockSettings>(configuration.GetSection(nameof(LockSettings)))
                .AddConfiguration<TaskHistorySettings>(configuration.GetSection(nameof(TaskHistorySettings)));

            service = service.AddSingleton<ISchedulerDbContext, SchedulerDbContext>()
                .AddScoped<IJobService, MongoDBJobService>()
                .AddScoped<IJobTaskService, MongoDBJobTaskService>()
                .AddScoped<ILocker, MongoDBLocker>();

            return service;
        }

        private static IServiceCollection AddConfiguration<TConfig>(this IServiceCollection service, IConfiguration configuration) where TConfig : class
        {
            return service.AddSingleton(provider =>
            {
                var setting = Activator.CreateInstance<TConfig>();

                configuration.GetSection(typeof(TConfig).Name).Bind(setting);

                return setting;
            });
        }
    }
}

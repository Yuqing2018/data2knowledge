using MusicKG.DataManager.ModelTrainer.Actions;
using MusicKG.DataManager.ModelTrainer.Executors;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;


namespace MusicKG.DataManager.ModelTrainer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModelTrainer(this IServiceCollection services, IConfiguration configuration, Func<IServiceCollection, IConfigurationSection, IServiceCollection> customerExecutorRegister)
        {
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, CollectTraningDataAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, TrainModelAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, ServeModelAction>());
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IJobAction, UpdateTrainingHistoryAction>());

            var section = configuration.GetSection("ModelTrainer");

            return customerExecutorRegister == null ? services : customerExecutorRegister.Invoke(services, section);
        }
    }
}
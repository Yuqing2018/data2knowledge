using MusicKG.Scheduler.Engine;
using MusicKG.Scheduler.Executor.Settings;
using MusicKG.Scheduler.Job;
using MusicKG.Scheduler.Service;
using MusicKG.Scheduler.Service.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Executor
{
    public class JobWorker : IHostedService
    {
        private readonly SchedulerSettings settings;
        private readonly IServiceProvider serviceProvider;
        private readonly IJobService jobService;
        private readonly ILogger<JobWorker> logger;
        private readonly List<IServiceScope> serviceScopes;

        public JobWorker(IServiceProvider serviceProvider, SchedulerSettings settings,
            IJobService jobService, ILogger<JobWorker> logger)
        {
            this.settings = settings;
            this.serviceProvider = serviceProvider;
            this.jobService = jobService;
            this.logger = logger;
            serviceScopes = new List<IServiceScope>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var jobs = await jobService.ListAsync(settings.SupportedJobs);

            if (jobs == null || jobs.Count == 0)
            {
                logger.LogWarning("There is no jobs.");
                return;
            }

            var workers = new List<Task>();

            foreach (var job in jobs)
            {
                if (!string.IsNullOrWhiteSpace(job.Schedule))
                    workers.Add(CreateScheduledJob(job, cancellationToken));
                else
                    workers.Add(CreateNormalJob(job, cancellationToken));
            }

            await Task.WhenAll(workers);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var scope in serviceScopes)
                scope.Dispose();
        }

        private async Task CreateNormalJob(JobServiceModel jobDefine, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var engine = scope.ServiceProvider.GetRequiredService<IJobEngine>();
                    await engine.ExecuteAsync(jobDefine);
                }

                await Task.Delay(TimeSpan.FromSeconds(settings.RefreshSleepIntervalInSeconds));
            }
        }

        private async Task CreateScheduledJob(JobServiceModel jobDefine, CancellationToken cancellationToken)
        {
            var scope = serviceProvider.CreateScope();

            var schedulerFactory = scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();

            var jobFactory = scope.ServiceProvider.GetRequiredService<IJobFactory>();

            var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

            scheduler.JobFactory = jobFactory;

            var jobId = jobDefine.Id;
            var jobSchedule = jobDefine.Schedule;

            string jobName = $"{settings.Name}_{jobId}", jobGroup = $"{settings.Name}";

            var job = JobBuilder.Create<ScheduledJob>().WithIdentity(jobName, jobGroup).Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName, jobGroup)
                .StartNow()
                .WithSchedule(CronScheduleBuilder.CronSchedule(jobSchedule)).Build();

            job.JobDataMap.Add("Job", jobDefine);

            await scheduler.ScheduleJob(job, trigger, cancellationToken).ConfigureAwait(true);

            await scheduler.Start().ConfigureAwait(true);

            serviceScopes.Add(scope);
        }
    }
}

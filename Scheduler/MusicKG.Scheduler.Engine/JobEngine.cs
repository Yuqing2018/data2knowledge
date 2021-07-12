using MusicKG.Scheduler.Engine.Settings;
using MusicKG.Scheduler.Service;
using MusicKG.Scheduler.Service.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Engine
{
    public class JobEngine : IJobEngine
    {
        private readonly JobSettings settings;
        private readonly IJobTaskExecutor executor;
        private readonly IJobTaskService jobTaskService;
        private readonly IJobService jobService;
        private readonly ILogger<JobEngine> logger;

        public JobEngine(
            JobSettings settings,
            IJobTaskExecutor executor,
            IJobTaskService jobTaskService,
            IJobService jobService,
            ILogger<JobEngine> logger)
        {
            this.executor = executor;
            this.jobTaskService = jobTaskService;
            this.settings = settings;
            this.jobService = jobService;
            this.logger = logger;
        }

        public async Task ExecuteAsync(JobServiceModel job)
        {
            var jobId = job.Id;

            var currentUtcTime = DateTime.UtcNow;

            if (job == null)
            {
                logger.LogWarning($"Job '{jobId}' is not defined.");
            }
            else
            {
                logger.LogInformation($"[{job.Name}]: Job started...");

                var allTasks = await jobTaskService.ListPreparedAsync(job.Id, settings.TaskMaxTryTimes);

                logger.LogInformation($"[{job.Name}]: {allTasks?.Count} tasks will be executed: '{string.Join(",", allTasks.Select(t => t.Name))}'...");

                for (int i = 0; i < allTasks?.Count; i += settings.MaxWorker)
                {
                    var tasks = allTasks.Skip(i).Take(settings.MaxWorker).ToList();

                    var workers = tasks.Select(task =>
                    {
                        if (task.LastSucceedAt == null || (task.LastSucceedAt != null && task.LastSucceedAt < currentUtcTime))
                            return executor.ExecuteAsync(job, task);
                        else
                            return Task.Run(() => { logger.LogInformation($"[{job.Name}] - [{task.Name}]: Task already executed by other worker..."); });
                    });

                    await Task.WhenAll(workers);
                }

                await jobService.UpdateRunTimeAsync(jobId, currentUtcTime);

                logger.LogInformation($"[{job.Name}]: All tasks were executed, the job finished.");
            }
        }
    }
}

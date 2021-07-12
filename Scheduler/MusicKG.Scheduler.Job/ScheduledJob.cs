using Microsoft.Extensions.Logging;
using MusicKG.Scheduler.Engine;
using MusicKG.Scheduler.Service.Models;
using Quartz;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Job
{
    public class ScheduledJob : IJob
    {
        private readonly IJobEngine engine;
        private readonly ILogger<ScheduledJob> logger;

        public ScheduledJob(
            IJobEngine engine,
            ILogger<ScheduledJob> logger)
        {
            this.engine = engine;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var job = context.JobDetail.JobDataMap["Job"] as JobServiceModel;

            if (job == null)
            {
                logger.LogWarning("There is no scheduled job configured.");
                return;
            }

            await engine.ExecuteAsync(job);
        }
    }
}

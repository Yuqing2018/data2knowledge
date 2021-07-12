using System.Collections.Generic;

namespace MusicKG.Scheduler.Executor.Settings
{
    public class SchedulerSettings
    {
        public string Name { get; set; } = "ScheduledJob";

        public int RefreshSleepIntervalInSeconds { get; set; } = 15;

        public List<string> SupportedJobs { get; set; } = new List<string>();
    }
}

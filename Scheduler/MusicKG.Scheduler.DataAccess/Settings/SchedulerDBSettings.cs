namespace MusicKG.Scheduler.DataAccess.Settings
{
    public class SchedulerDBSettings
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public string LockerCollectionName { get; set; } = "SchedulerLockers";

        public string JobDefineCollectionName { get; set; } = "ScheduledJobs";

        public string JobTaskCollectionName { get; set; } = "ScheduledJobTasks";
    }
}

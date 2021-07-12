namespace MusicKG.Scheduler.Service.Settings
{
    public class WorkerSettings
    {
        /// <summary>
        /// Worker ping interval in milliseconds
        /// </summary>
        public int PingIntervalInMilliseconds { get; set; } = 20000;

        /// <summary>
        /// Worker liveness threshold in milliseconds
        /// </summary>
        public int LivenessThresholdInMilliseconds { get; set; } = 60000;

        /// <summary>
        /// Worker interval in milliseconds when idle
        /// </summary>
        public int IntervalInMillisecondsWhenIdle { get; set; } = 10000;

        /// <summary>
        /// Worker interval in milliseconds when busy
        /// </summary>
        public int IntervalInMillisecondsWhenBusy { get; set; } = 0;
    }
}

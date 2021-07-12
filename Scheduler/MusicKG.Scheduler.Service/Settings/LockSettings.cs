namespace MusicKG.Scheduler.Service.Settings
{
    public class LockSettings
    {
        /// <summary>
        /// How often (in seconds) to perform the heartbeat
        /// Default 30 seconds
        /// </summary>
        public int LivnessProbePeriodSeconds { get; set; } = 30;

        /// <summary>
        /// Consider a minion is dead if not seen in LivnessFailureThreshold * LivnessProbePeriodSeconds
        /// Default 3 times
        /// </summary>
        public int LivnessFailureThreshold { get; set; } = 3;
    }
}

namespace MusicKG.Scheduler.Engine.Settings
{
    public class JobSettings
    {
        public int MaxWorker { get; set; } = 1;

        public int TaskMaxTryTimes { get; set; } = 3;
    }
}
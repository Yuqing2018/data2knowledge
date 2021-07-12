namespace MusicKG.Scheduler.Engine.Models
{
    public class JobActionOptions
    {
        public const string DefaultExecutorName = "Default";

        public string ExecutorName { get; set; } = DefaultExecutorName;

        public bool Ignore { get; set; } = false;
    }
}

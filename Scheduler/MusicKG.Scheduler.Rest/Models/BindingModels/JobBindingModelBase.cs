namespace MusicKG.Scheduler.Rest.Models.BindingModels
{
    public class JobBindingModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Schedule { get; set; }

        public string JobType { get; set; }
    }
}

using System;

namespace MusicKG.Scheduler.Rest.Models.ViewModels
{
    public class JobViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string JobType { get; set; }

        public string Description { get; set; }

        public string Schedule { get; set; }

        public DateTime? LastRunAt { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MusicKG.Scheduler.Service.Models
{
    public class JobServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public string JobType { get; set; }

        public string Schedule { get; set; }

        public List<JobActionServiceModel> Actions { get; set; }

        public DateTime? LastRunAt { get; set; }
    }
}
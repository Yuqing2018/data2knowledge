using MusicKG.Scheduler.DataAccess.Enums;
using System;

namespace MusicKG.HondaPlugins.Services.Models.ReannotationTasks
{
    public class DataReannotationTaskServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public TaskExecutionResult Status { get; set; }

        public DateTime? SucceedAt { get; set; }
    }
}

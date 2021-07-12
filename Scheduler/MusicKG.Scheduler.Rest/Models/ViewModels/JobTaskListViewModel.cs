using MusicKG.Scheduler.DataAccess.Enums;
using System;

namespace MusicKG.Scheduler.Rest.Models.ViewModels
{
    public class JobTaskListViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public JobViewModel Job { get; set; }

        public string Description { get; set; }

        public bool IsRunOnce { get; set; }

        public string Executor { get; set; }

        public DateTime ManagedAt { get; set; }

        public DateTime MerchantCreationTime { get; set; }

        public string ManagedBy { get; set; }

        public TaskExecutionResult LastStatus { get; set; }

        public DateTime? LastSucceedAt { get; set; }

        public DateTime? LastRunAt { get; set; }
    }
}

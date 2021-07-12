using System;
namespace MusicKG.Scheduler.Rest.Models.BindingModels
{
    public class JobTaskBindingModelBase
    {
        public string JobId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRunOnce { get; set; }

        public string ExecutorDomain { get; set; }

        public string ExecutorName { get; set; }

        public string ExecutorPassword { get; set; }

        public DateTime MerchantCreationTime { get; set; }

        public string ManagedBy { get; set; }
    }
}

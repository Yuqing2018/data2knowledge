using Newtonsoft.Json.Linq;
using System;

namespace MusicKG.Scheduler.Service.Models
{
    public class JobTaskManagementServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRunOnce { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string ExecutorDomain { get; set; }

        public string ExecutorName { get; set; }

        public string ExecutorPassword { get; set; }

        public DateTime MerchantCreationTime { get; set; }

        public string ManagedBy { get; set; }

        public JObject TaskDefine { get; set; }
    }
}

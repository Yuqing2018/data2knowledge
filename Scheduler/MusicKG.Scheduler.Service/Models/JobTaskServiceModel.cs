using MongoDB.Bson;
using MusicKG.Scheduler.DataAccess.Enums;
using System;

namespace MusicKG.Scheduler.Service.Models
{
    public class JobTaskServiceModel
    {
        public string Id { get; set; }

        public JobServiceModel Job { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRunOnce { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Executor { get; set; }

        public string ExecutorPassword { get; set; }

        public string ExecutorDomain { get; set; }

        public DateTime MerchantCreationTime { get; set; }

        public DateTime ManagedAt { get; set; }

        public string ManagedBy { get; set; }

        public bool IsDeleted { get; set; }

        public TaskExecutionResult LastStatus { get; set; }

        public DateTime? LastRunAt { get; set; }

        public DateTime? LastSucceedAt { get; set; }

        public BsonDocument TaskDefine { get; set; }
    }
}

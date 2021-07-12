using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MusicKG.Scheduler.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class JobTaskDataModel
    {
        [BsonId]
        public string Id { get; set; }

        public string JobId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRunOnce { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public ExecutorDataModel Executor { get; set; }

        public DateTime MerchantCreationTime { get; set; }

        public DateTime ManagedAt { get; set; }

        public string ManagedBy { get; set; }

        public bool IsDeleted { get; set; }

        public BsonDocument TaskDefine { get; set; }

        public JobTaskStatus Status { get; set; }
    }
}

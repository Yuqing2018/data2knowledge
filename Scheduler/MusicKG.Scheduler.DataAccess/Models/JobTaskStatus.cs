using MusicKG.Scheduler.DataAccess.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MusicKG.Scheduler.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class JobTaskStatus
    {
        [BsonRepresentation(BsonType.String)]
        public TaskExecutionResult LastStatus { get; set; }

        public DateTime? LastRunAt { get; set; }

        public DateTime? LastFinishedAt { get; set; }

        public DateTime? LastSucceedAt { get; set; }

        public int TryTimes { get; set; }

        public List<JobTaskHistory> History { get; set; }
    }
}

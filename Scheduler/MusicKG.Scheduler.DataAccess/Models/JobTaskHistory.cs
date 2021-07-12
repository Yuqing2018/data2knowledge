using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MusicKG.Scheduler.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class JobTaskHistory
    {
        public DateTime RunAt { get; set; }

        public DateTime FinishedAt { get; set; }

        public bool IsSucceed { get; set; }

        public string DeadAction { get; set; }

        public string Message { get; set; }
    }
}

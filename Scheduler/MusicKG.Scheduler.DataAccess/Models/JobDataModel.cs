using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MusicKG.Scheduler.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class JobDataModel
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string JobType { get; set; }

        public string Description { get; set; }

        public string Schedule { get; set; }

        public List<JobActionDataModel> Actions { get; set; }

        public DateTime? LastRunAt { get; set; }
    }
}

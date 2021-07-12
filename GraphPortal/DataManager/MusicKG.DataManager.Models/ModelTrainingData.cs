using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MusicKG.DataManager.Models
{
    public class ModelTrainingData
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        public string ModelName { get; set; }

        public string DataSource { get; set; }

        public Dictionary<string, string> InputFeatures { get; set; }

        public Dictionary<string, string> OutputFeatures { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
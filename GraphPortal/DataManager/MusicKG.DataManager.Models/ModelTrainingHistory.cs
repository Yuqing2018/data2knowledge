using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MusicKG.DataManager.Models
{
    [BsonIgnoreExtraElements]
    public class ModelTrainingHistory
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string ModelName { get; set; }

        public int CurrentVersion { get; set; }

        public List<ModelVersion> ModelVersions { get; set; }
    }
}

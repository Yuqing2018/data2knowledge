using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class TaskIAAScoreDataModel
    {
        public ObjectId Id { get; set; }

        public IEnumerable<TaskEntityTypeScoreDataModel> EntityTypes { get; set; }

        public IEnumerable<TaskRelationshipScoreDataModel> Relationships { get; set; }

        public IEnumerable<TaskCoreferenceScoreDataModel> Coreferences { get; set; }

        public double Score { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

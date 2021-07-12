using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class TaskEntityTypeScoreDataModel
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public double Score { get; set; }
    }
}

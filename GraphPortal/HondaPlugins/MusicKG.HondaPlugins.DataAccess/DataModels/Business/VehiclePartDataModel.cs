using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Business
{
    public class VehiclePartDataModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public string No { get; set; }

        public List<string> Names { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

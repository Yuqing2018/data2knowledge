using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class CategoryDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public string Name { get; set; }

        public ObjectId CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

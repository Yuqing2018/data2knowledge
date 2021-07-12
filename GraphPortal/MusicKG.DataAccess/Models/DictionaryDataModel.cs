using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class DictionaryDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public string Name { get; set; }

        public ObjectId? EntityId { get; set; }

        public List<string> Vocabularies { get; set; }

        public ObjectId CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}

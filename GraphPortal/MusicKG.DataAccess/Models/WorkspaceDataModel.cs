using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class WorkspaceDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ObjectId Type { get; set; }

        public LanguageEnum Language { get; set; }

        public bool IsAutoMerging { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAutoCreateTask { get; set; } = false;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ResultHandlerAssembly { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ResultHandlerClass { get; set; }

        public ObjectId CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }
        
        public List<ObjectId> ReadOnlyUserIds { get; set; }
    }
}

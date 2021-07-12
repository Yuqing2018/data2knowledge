using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.DataAccess.Models
{
    /// <summary>
    /// Regex Rule Data Model.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class RuleDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public ObjectId EntityId { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public List<string> Regexs { get; set; }
    }
}

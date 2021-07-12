using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class OntologyEntityDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public string Name { get; set; }

        public List<OntologyEntityPropertyDataModel> Properties { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }
    }
}

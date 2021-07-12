using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class OntologyRelationDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId WorkspaceId { get; set; }

        public string Name { get; set; }

        public ObjectId FirstEntityId { get; set; }

        public ObjectId SecondEntityId { get; set; }

        public List<OntologyRelationPropertyDataModel> Properties { get; set; }

        public string Description { get; set; }
    }
}

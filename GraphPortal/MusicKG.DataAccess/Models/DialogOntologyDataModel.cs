using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class DialogOntologyDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId? IntentDocumentId { get; set; }

        public ObjectId? EntityDocumentId { get; set; }

        public ObjectId WorkspaceId { get; set; }
    }
}

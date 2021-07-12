using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class OntologyEntityPropertyTypeDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
    }
}

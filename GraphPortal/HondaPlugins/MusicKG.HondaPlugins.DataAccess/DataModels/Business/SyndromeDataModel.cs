using MusicKG.HondaPlugins.DataAccess.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Business
{
    [BsonIgnoreExtraElements]
    public class SyndromeDataModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public BadGrade BadGrade { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

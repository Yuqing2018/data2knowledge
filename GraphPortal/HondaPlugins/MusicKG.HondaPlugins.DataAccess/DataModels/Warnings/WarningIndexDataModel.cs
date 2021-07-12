using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MusicKG.HondaPlugins.DataAccess.Enums;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    public class WarningIndexDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public WarningType WarningType { get; set; }
        [BsonRepresentation(BsonType.String)]
        public WarningIndexNames IndexName { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }
}

using MusicKG.HondaPlugins.DataAccess.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Business
{
    public class AIRiskWarningLevelMappingDataModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public RiskLevel RiskLevel { get; set; }

        public uint RiskLevelInModel { get; set; }
    }
}

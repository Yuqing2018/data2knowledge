using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.Scheduler.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class JobActionDataModel
    {
        public string ActionId { get; set; }

        public string Description { get; set; }

        public bool IsDefault { get; set; }

        public BsonDocument Options { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.DataAccess.Models
{
    [BsonIgnoreExtraElements]
    public class OntologyEntityPropertyDataModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public ObjectId Type { get; set; }
    }
}

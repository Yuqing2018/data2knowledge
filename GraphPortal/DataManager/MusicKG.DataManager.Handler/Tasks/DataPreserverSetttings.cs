using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.DataManager.Translator.Tasks
{
    [BsonIgnoreExtraElements]
    public class DataPreserverSetttings
    {
        public string ServiceUrl { get; set; }
    }
}

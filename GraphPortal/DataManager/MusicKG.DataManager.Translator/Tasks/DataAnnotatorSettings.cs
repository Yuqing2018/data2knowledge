using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.DataManager.Translator.Tasks
{
    [BsonIgnoreExtraElements]
    public class DataAnnotatorSettings
    {
        public string AnnotationServiceEndpoint { get; set; }

        public string ModelVersion { get; set; }

        public int ItemsCountPerRequest { get; set; } = 20;
    }
}

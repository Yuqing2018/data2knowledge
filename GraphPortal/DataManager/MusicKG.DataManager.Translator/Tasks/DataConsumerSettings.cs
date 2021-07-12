using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicKG.DataManager.Translator.Tasks
{
    [BsonIgnoreExtraElements]
    public class DataConsumerSettings
    {
        public List<string> TagFeatures { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DateTimeFormat { get; set; } = "yyyyMMddHHmmss";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; } = "application/json";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ItemsPerDocument { get; set; } = 50;
    }
}
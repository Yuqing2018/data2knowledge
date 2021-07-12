using MongoDB.Bson.Serialization.Attributes;
using MusicKG.DataManager.Translator.Settings;
using Newtonsoft.Json;

namespace MusicKG.DataManager.Translator.Tasks
{
    /// <summary>
    /// Customerized task defination.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class DataTranslationTaskDefine
    {
        public string DataSourceName { get; set; }

        public string WorkspaceId { get; set; }

        public DefaultDbSetting DBSettings { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataConsumerSettings DataConsumerSettings { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataAnnotatorSettings DataAnnotatorSettings { get; set; }
    }
}

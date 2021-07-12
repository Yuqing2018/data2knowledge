using MongoDB.Bson.Serialization.Attributes;

namespace MusicKG.DataManager.Translator.Tasks
{
    [BsonIgnoreExtraElements]
    public class DataSourceSettings
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public string TableName { get; set; }

        public string TimestampFieldName { get; set; }
    }
}
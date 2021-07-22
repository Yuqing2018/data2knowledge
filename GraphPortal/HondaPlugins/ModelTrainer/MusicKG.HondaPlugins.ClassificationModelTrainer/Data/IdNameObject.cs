using MongoDB.Bson;
using Newtonsoft.Json;

namespace MusicKG.HondaPlugins.ModelTrainer.Data
{
    public class IdNameObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public ObjectId MongoId
        {
            set
            {
                Id = value.ToString();
            }
        }
    }

}

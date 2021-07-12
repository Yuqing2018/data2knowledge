using Newtonsoft.Json;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Labeling
{
    public class PredictResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double Probability { get; set; }
    }
}

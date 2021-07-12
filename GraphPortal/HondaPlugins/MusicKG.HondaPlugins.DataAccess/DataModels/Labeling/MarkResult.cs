using Newtonsoft.Json;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Labeling
{
    public class MarkResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAddForTraining { get; set; } = false;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PredictResult PartName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PredictResult Syndrome { get; set; }
    }
}

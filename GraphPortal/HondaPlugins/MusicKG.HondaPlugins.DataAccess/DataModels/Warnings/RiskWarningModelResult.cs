using MusicKG.HondaPlugins.DataAccess.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Warnings
{
    public class RiskWarningModelResult
    {
        [JsonProperty("IsSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("predict_label")]
        public List<uint> PredictLabel { get; set; }
    }
}

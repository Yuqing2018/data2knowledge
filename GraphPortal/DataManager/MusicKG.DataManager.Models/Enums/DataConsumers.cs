using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicKG.DataManager.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataConsumers
    {
        LabelingTool,

        Model,

        Business
    }
}
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MusicKG.DataManager.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataManagerJobTypes
    {
        DataTranslateToLabelingTool,

        DataTranslateToMasterDB,

        DataReannotation,

        DataHandling
    }
}

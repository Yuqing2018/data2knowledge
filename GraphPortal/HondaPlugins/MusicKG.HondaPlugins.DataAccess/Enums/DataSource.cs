using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicKG.HondaPlugins.DataAccess.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataSource
    {
        MQI,

        TECH_CONSULTING,

        HOTLINE,

        GOV,

        MEDIA_MAIN,

        MEDIA_SUB
    }
}

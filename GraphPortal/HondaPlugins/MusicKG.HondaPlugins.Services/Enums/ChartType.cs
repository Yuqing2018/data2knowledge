using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.HondaPlugins.Services.Enums
{
    public enum ChartType
    {
        生产,
        故障,
        经过,
        渠道
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataSourceType
    {
        MQI,
        技术咨询,
        热线800,
        总局,
        网络媒体
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FocusType
    {
        已关注,
        已退出,
        未关注
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatisticalFrequency
    {
        日,
        周,
        月
    }
}

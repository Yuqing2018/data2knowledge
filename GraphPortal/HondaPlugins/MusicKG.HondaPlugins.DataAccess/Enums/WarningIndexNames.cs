using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.HondaPlugins.DataAccess.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WarningIndexNames
    {
        近三个月相对上升率,
        近三个月发生件数,
        年款不良率,
        风险等级,
        对策后生产车辆的MQI件数, 
        对策后再发不良率
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.HondaPlugins.DataAccess.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BadGrade
    {
        A,
        B,
        C
    }
}

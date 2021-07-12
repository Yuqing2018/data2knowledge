using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Enums
{
    /// <summary>
    /// Document content output type enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DocumentContentOutputTypeEnum
    {
        /// <summary>
        /// Text.
        /// </summary>
        Text = 0,

        /// <summary>
        /// Raw.
        /// </summary>
        Raw = 1
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Enums
{
    /// <summary>
    /// Entity output format enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntityOutputFormatEnum
    {
        /// <summary>
        /// Object.
        /// </summary>
        Object = 0,

        /// <summary>
        /// SPO.
        /// </summary>
        SPO = 1
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// Language enum
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LanguageEnum
    {
        /// <summary>
        /// Chinese
        /// </summary>
        Chinese = 0,

        /// <summary>
        /// English
        /// </summary>
        English = 1
    }
}
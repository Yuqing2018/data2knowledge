using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// User status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserStatusEnum
    {
        /// <summary>
        /// Enabled.
        /// </summary>
        Enabled = 0,

        /// <summary>
        /// Disabled.
        /// </summary>
        Disabled = 1
    }
}

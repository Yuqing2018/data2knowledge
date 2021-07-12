using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// Document status enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DocumentStatusEnum
    {
        /// <summary>
        /// Uploaded.
        /// </summary>
        Uploaded = 1,

        /// <summary>
        /// Preprocessed.
        /// </summary>
        Preprocessed = 2,

        /// <summary>
        /// Preannotated.
        /// </summary>
        Preannotated = 3,

        /// <summary>
        /// Assigned.
        /// </summary>
        Assigned = 4
    }
}

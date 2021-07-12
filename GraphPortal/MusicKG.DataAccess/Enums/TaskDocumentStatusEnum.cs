using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// Task Document status enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskDocumentStatusEnum
    {
        /// <summary>
        /// Assigned.
        /// </summary>
        Assigned = 0,

        /// <summary>
        /// Annotated.
        /// </summary>
        Annotated = 1
    }
}

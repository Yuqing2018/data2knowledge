using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Enums
{
    /// <summary>
    /// Workflow task status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkflowTaskStatusEnum
    {
        /// <summary>
        /// Workflow task stopped.
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// Workflow task started.
        /// </summary>
        Started = 1
    }
}

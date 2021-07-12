using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Enums
{
    /// <summary>
    /// Workflow step actions.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkflowStepActionEnum
    {
        /// <summary>
        /// Create processor action.
        /// </summary>
        CreatingProcessor = 0,

        /// <summary>
        /// Fetching document action.
        /// </summary>
        FetchingDocuments = 1,

        /// <summary>
        /// Waiting for documents action.
        /// </summary>
        WaitingForDocuments = 2,

        /// <summary>
        /// Processing documents action.
        /// </summary>
        ProcessingDocuments = 3,

        /// <summary>
        /// Updating parent documents action.
        /// </summary>
        UpdatingParentDocuments = 4
    }
}

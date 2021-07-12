using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// Option type enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OptionTypeEnum
    {
        /// <summary>
        /// User role.
        /// </summary>
        UserRole = 0,

        /// <summary>
        /// AI model.
        /// </summary>
        Model = 2,

        /// <summary>
        /// Language.
        /// </summary>
        Language = 3,

        /// <summary>
        /// Workspace type.
        /// </summary>
        WorkspaceType = 4,

        /// <summary>
        /// Workflow.
        /// </summary>
        Workflow = 5,

        /// <summary>
        /// Entity property type.
        /// </summary>
        EntityPropertyType = 6,

        /// <summary>
        /// Document status.
        /// </summary>
        DocumentStatus = 7,

        /// <summary>
        /// Task status.
        /// </summary>
        TaskStatus = 8,

        /// <summary>
        /// Task document status.
        /// </summary>
        TaskDocumentStatus = 9,

        /// <summary>
        /// Task types.
        /// </summary>
        TaskTypes = 10,
    }
}

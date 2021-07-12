using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// Task Status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskStatusEnum
    {
        /// <summary>
        /// Created.
        /// </summary>
        Created = 0,

        /// <summary>
        /// Submitted.
        /// </summary>
        Submitted = 10,

        /// <summary>
        /// Accepted.
        /// </summary>
        Accepted = 20,

        /// <summary>
        /// Rejected.
        /// </summary>
        Rejected = 30,

        /// <summary>
        /// Annotation result conflict resolved.
        /// </summary>
        ConflictResolved = 40,

        /// <summary>
        /// Knowledge merged into graph database.
        /// </summary>
        KnowledgeMerged = 50
    }
}

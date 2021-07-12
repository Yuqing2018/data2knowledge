using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// Task Document ResultType status enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskDocumentResultTypeEnum
    {
        /// <summary>
        /// For Model Training.
        /// </summary>
        ForModelTraining = 0,

        /// <summary>
        /// For Graph Merging.
        /// </summary>
        ForGraphMerging = 1
    }
}

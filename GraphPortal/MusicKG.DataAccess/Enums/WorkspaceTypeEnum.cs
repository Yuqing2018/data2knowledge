
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataAccess.Enums
{
    /// <summary>
    /// Workspace type enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkspaceTypeEnum
    {
        /// <summary>
        /// Knowledge extract workspace.
        /// </summary>
        KnowledgeExtract = 0,

        /// <summary>
        /// Text similarity workspace.
        /// </summary>
        TextSimilarity = 1,

        /// <summary>
        /// Tokenization workspace.
        /// </summary>
        Tokenization = 2,

        /// <summary>
        /// Text paraphrase workspace (planning).
        /// </summary>
        TextParaphrase = 3,

        /// <summary>
        /// Named entity recognition workspace (planning).
        /// </summary>
        NamedEntityRecognition = 4
    }
}

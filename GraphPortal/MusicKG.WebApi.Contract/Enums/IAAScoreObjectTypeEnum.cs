using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicKG.WebApi.Contract.Enums
{
    /// <summary>
    /// IAA score object type enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IAAScoreObjectTypeEnum
    {
        /// <summary>
        /// Entity.
        /// </summary>
        Entity = 0,

        /// <summary>
        /// Relationship.
        /// </summary>
        Relationship = 1,

        /// <summary>
        /// Coreference.
        /// </summary>
        Coreference = 2
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicKG.WebApi.Contract.Enums
{
    /// <summary>
    /// IAA score view type enum.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IAAScoreViewTypeEnum
    {
        /// <summary>
        /// Annotator.
        /// </summary>
        Annotator = 0,

        /// <summary>
        /// Document.
        /// </summary>
        Document = 1
    }
}

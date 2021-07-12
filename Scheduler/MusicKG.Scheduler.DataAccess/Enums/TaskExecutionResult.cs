using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MusicKG.Scheduler.DataAccess.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskExecutionResult
    {
        准备中,
        执行中,
        执行成功,
        执行失败
    }
}

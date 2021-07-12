using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace MusicKG.HondaPlugins.DataAccess.Enums
{
    /// <summary>
    /// 预警类别
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WarningType
    {
        多发预警,
        风险预警,
        再发预警
    }
    /// <summary>
    /// 预警单元
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WarningUnit
    {
        零件_不良症状,
        未知零件_不良症状,
        不良症状
    }
    /// <summary>
    /// 预警任务状态
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WarningTaskStatus
    {
        预警中,
        已终止,
    }
    /// <summary>
    /// 推进分类，推进分类
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PushStatus
    {
        未立项,
        需再立项,
        推进中, 
        监视,
        完了,
        其他
    }
    /// <summary>
    /// 对策状态
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermanentCntrStatus
    {
        未明确,
        已明确,
        无需对策,
        已对策,
        空白
    }
    /// <summary>
    /// 预警频率
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WarningFrequency
    {
        每日,
        每周,
        每月,
        每季,
        指定日期,
        不报警
    }
    /// <summary>
    /// 风险等级
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RiskLevel
    {
        重点关注,
        高风险,
        潜在高风险,
        中风险,
        其他关注,
        低风险,
        一般
    }
    /// <summary>
    /// 处理状态
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProcessStatus
    {
        待处理,
        已处理
    }
}

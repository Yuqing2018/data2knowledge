using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.DataModels.Statistics
{
    public class StatisticsDataModel
    {
        /// <summary>
        /// 车款
        /// </summary>
        public string CarModel { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarType { get; set; }

        /// <summary>
        /// 年款
        /// </summary>
        public string ModelYear { get; set; }

        /// <summary>
        /// 零件名
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 不良症状Id
        /// </summary>
        public string Syndrome { get; set; }

        /// <summary>
        /// 故障总数
        /// </summary>
        public long FaultCount { get; set; }

        /// <summary>
        /// 故障车架号
        /// </summary>
        public List<string> FaultFrameNo { get; set; }

        /// <summary>
        /// 关联信息中的对策日期
        /// 对策日期
        /// </summary>
        public List<DateTime> MeasuresDates { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public DataSource DataSource { get; set; }

        /// <summary>
        /// 所有故障车辆的生产日期
        /// </summary>
        public List<DateTime> ProductionDates { get; set; }

        /// <summary>
        /// 所有故障车辆的销售日期
        /// </summary>
        public List<DateTime> SaleDates { get; set; }

        /// <summary>
        /// 所有故障车辆的故障日期
        /// </summary>
        public List<DateTime> FaultDates { get; set; }

        /// <summary>
        /// 该车款车型年款的日销量
        /// Key - 从售出第一辆开始的日期
        /// Value - 该日销量
        /// </summary>
        public Dictionary<DateTime, long> DailySales { get; set; }

        /// <summary>
        /// 从最早的生产日期开始后每天的故障数
        /// Key - 生产日期
        /// Value - 故障总数
        /// </summary>
        public Dictionary<DateTime, long> DailyFaultCount { get; set; }

        /// <summary>
        /// 从购买日期到故障日期的天数统计故障数
        /// Key - 故障日期-生产日期（天）
        /// Value - 故障数
        /// </summary>
        public Dictionary<int, long> FaultCountByPassMonth { get; set; }

        /// <summary>
        /// 从Key日期到售出第一辆车的日期的不良率（从Key日期到售出第一辆车的日期的故障数/从Key日期到售出第一辆车的日期的总销量）
        /// Key - 从售出第一辆车的日期开始到现在的每一天
        /// Value - 不良率（从售出第一辆车的日期到key的日期）
        /// </summary>
        public Dictionary<DateTime, float> DailyFaultRateByFromFirtSaleDate { get; set; }

        /// <summary>
        /// Key - 里程数（5000km为单位）
        /// Value - 故障总数
        /// </summary>
        public Dictionary<int, long> FaultCountByMileAge { get; set; }

        /// <summary>
        /// Key - 特约店
        /// Value - 故障总数
        /// </summary>
        public Dictionary<string, long> FaultCountByDealerCD { get; set; }

        /// <summary>
        /// Key - 片区
        /// Value - 故障总数
        /// </summary>
        public Dictionary<string, long> FaultCountByRegion { get; set; }
    }
}

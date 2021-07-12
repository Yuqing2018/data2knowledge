using MusicKG.HondaPlugins.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.HondaPlugins.Services.Models
{
    public class StatisticServiceModel
    {
        /// <summary>
        /// 生产分布
        /// Dictionary:
        ///     key:不良症状
        ///     Value:月不良件数
        /// </summary>
        public Dictionary<string,List<MonthMetric>> ProductDistribution { get; set; }
        /// <summary>
        /// 故障
        /// </summary>
        public Dictionary<string, List<MonthMetric>> FaultDistribution { get; set; }
        /// <summary>
        /// 经过
        /// </summary>
        public Dictionary<string, List<MonthMetric>> FaultSpanDistribution { get; set; }
        /// <summary>
        /// 里程
        /// </summary>
        public Dictionary<int, int> MileAgeDistribution { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public List<RegionMetric> RegionDistribution { get; set; }
        /// <summary>
        /// 特约店
        /// key：年款
        /// Value:
        ///     key:特约店
        ///     value:不良件数
        /// </summary>
        public Dictionary<string, Dictionary<string, int>> DealerDistribution { get; set; }
        /// <summary>
        /// 按经过月不良率
        /// key：年款
        /// Value: 
        /// </summary>
        public Dictionary<string, List<MonthRateMetric>> DefectRateDistribution { get; set; }
        /// <summary>
        /// 按对策时间的不良率分布
        /// </summary>
        public Dictionary<string, List<MonthRateByCntrMetric>> DefectRateByCntrDistribution { get; set; }
        /// <summary>
        /// key:渠道
        /// value:具体不良分布
        /// </summary>
        public Dictionary<string, List<MonthMetric>> DataSourceDistribution { get; set; }

    }
    public class CircularChartMetrics<T>
    {
        public string CarModel { get; set; }
        public int TotalCount { get; set; }
        public List<T> Metrics { get; set; }
    }

    public class SyndromeMetric
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RiskLevel { get; set; }
        public int TotalCount { get; set; }
    }

    public class PartNameMetric
    {
        public string Name { get; set; }
        public int TotalCount { get; set; }
    }

    public class MonthRateMetric
    {
        public string Month { get; set; }
        public float DefectRate { get; set; }
    }
    public class MonthRateByCntrMetric
    {
        public string Month { get; set; }
        public float DefectRate { get; set; }
        public DateTime? CntrTime { get; set; }
    }

    public class MonthMetric
    {
        public string Month { get; set; }
        public int TotalCount { get; set; }
    }

    public class DayMetric
    {
        /// <summary>
        /// 只有在渠道按日统计的时候赋值
        /// </summary>
        public string DataSource { get; set; }
        public string Day { get; set; }
        public int Count { get; set; }
    }

    public class DayRateMetric
    {
        public string Day { get; set; }
        public float DefectRate { get; set; }
    }

    public class RegionMetric
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public Dictionary<string, int> Top5Dealer { get;set;}
    }

}

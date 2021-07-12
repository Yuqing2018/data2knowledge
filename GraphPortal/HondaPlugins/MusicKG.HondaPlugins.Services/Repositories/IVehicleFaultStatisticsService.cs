using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories
{
    public interface IVehicleFaultStatisticsService
    {
        Task<StatisticServiceModel> CalculateMetrics(BaseSearchModel search);

        Task<List<CircularChartMetrics<SyndromeMetric>>> GetSyndromeMetrics(BaseSearchModel search);

        Task<List<PartNameMetric>> GetPartNameMetrics(BaseSearchModel search);

        Task<List<DayMetric>> MetricsInOneMonth(BaseSearchModel search, string month, ChartType chartType);

        Task<Dictionary<string, List<DayRateMetric>>> CntrRateMetricsInOneMonth(BaseSearchModel search, string month);

        Task<Dictionary<string, List<DayRateMetric>>> RateMetricsInOneMonth(BaseSearchModel search, string month);
        /// <summary>
        /// Key:年款
        /// Value:(单台件数统计表，单台金额统计表)
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<Dictionary<string, (List<List<string>> countTable, List<List<string>> amountTable)>> ExportDatas(BaseSearchModel search);
    }
}

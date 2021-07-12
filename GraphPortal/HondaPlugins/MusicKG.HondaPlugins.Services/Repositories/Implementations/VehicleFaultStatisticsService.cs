using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using MusicKG.HondaPlugins.DataAccess.Resources;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Enums;
using MusicKG.HondaPlugins.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Repositories.Implementations
{
    public partial class VehicleFaultStatisticsService : IVehicleFaultStatisticsService
    {
        private readonly IVehicleFaultDataService vehicleFaultDataService;
        private readonly IRawSalesService rawSaleService;
        private readonly IWarningRecordService warningRecordService;
        private readonly ILogger<VehicleFaultStatisticsService> logger;

        /// <summary>
        /// MQIRepository service constructor.
        /// </summary>
        /// <param name="context">MusicKG mongodb context.</param>
        public VehicleFaultStatisticsService(
            IVehicleFaultDataService vehicleFaultDataService,
            IRawSalesService rawSaleService,
            IWarningRecordService warningRecordService,
            ILogger<VehicleFaultStatisticsService> logger)
        {
            this.vehicleFaultDataService = vehicleFaultDataService;
            this.rawSaleService = rawSaleService;
            this.warningRecordService = warningRecordService;
            this.logger = logger;
        }

        public async Task<StatisticServiceModel> CalculateMetrics(BaseSearchModel search)
        {
            var result = new StatisticServiceModel();
            var all_dataList = await GetAllDataSourceAsync(search);
            var dataList = all_dataList.Where(x => x.DataSource != DataSource.MEDIA_SUB).ToList();
            if (search.Datasource.Count > 0)
            {
                dataList = dataList.Where(x => search.Datasource.Contains(x.DataSource)).ToList();
            }
            if (all_dataList.Count == 0 || dataList.Count == 0)
                return result;
            var all_sales_datas = await rawSaleService.GetSalesDatas(search.CarModel, search.CarType, search.YearModels, null, null, null);

            
            var syndromes = dataList.Select(x => x.SyndromeModel.Name).Distinct().ToList();
            #region 生产
            var minDate = dataList.Select(x => x.ProductionDate).Min();
            var maxDate = dataList.Select(x => x.ProductionDate).Max();
            var productYears = dataList.Select(x => x.ProductionDate.Year).Distinct().ToList();
            var productBySyndromesDict = dataList.GroupBy(x => x.SyndromeModel.Name).ToDictionary(k => k.Key, d =>
                 {
                     var dayMetric = d.GroupBy(x => x.ProductionDate.Date).ToDictionary(k => k.Key, v => v.Count());
                     var productMetricDict = dayMetric.GroupBy(x => x.Key.ToString("yyyy-MM")).ToDictionary(k => k.Key, v => v.Sum(x => x.Value));
                     return ReConstruct(productYears.Min(), productYears.Max(), minDate, maxDate, productMetricDict);
                 });
            result.ProductDistribution = syndromes.ToDictionary(k => k, v =>
               {
                   return productBySyndromesDict.ContainsKey(v) ? productBySyndromesDict[v] : ReConstruct(productYears.Min(), productYears.Max(), minDate, maxDate, null);
               });
            #endregion

            #region 故障
            var minDateOfFault = dataList.Select(x => x.FaultDate).Min();
            var maxDateOfFault = dataList.Select(x => x.FaultDate).Max();
            var faultYears = dataList.Select(x => x.FaultDate.Year).Distinct().ToList();
            var faultBySyndromesDict = dataList.GroupBy(x => x.SyndromeModel.Name).ToDictionary(k => k.Key, d =>
            {
                var dayMetric = d.GroupBy(x => x.FaultDate.Date).ToDictionary(k => k.Key, v => v.Count());
                var faultMetricDict = dayMetric.GroupBy(x => x.Key.ToString("yyyy-MM")).ToDictionary(k => k.Key, v => v.Sum(x => x.Value));
                return ReConstruct(faultYears.Min(), faultYears.Max(), minDateOfFault, maxDateOfFault, faultMetricDict);
            });
            result.FaultDistribution = syndromes.ToDictionary(k => k, v =>
            {
                return faultBySyndromesDict.ContainsKey(v) ? faultBySyndromesDict[v] : ReConstruct(faultYears.Min(), faultYears.Max(), minDateOfFault, maxDateOfFault, null);
            });
            #endregion

            #region 经过月
            result.FaultSpanDistribution = new Dictionary<string, List<MonthMetric>>();
            var faultDaySpanSyndromeDict = dataList.GroupBy(x => x.SyndromeModel.Name).ToDictionary(k => k.Key, v => v.GroupBy(x => (x.FaultDate - x.InitialRegistDate).TotalDays).ToDictionary(vk => vk.Key, vv => vv.Count()));
            var maxMonth = (int)Math.Ceiling((double)faultDaySpanSyndromeDict.Values.SelectMany(x => x.Keys).Max() / 30);
            var syndromeFaultMonthDict = faultDaySpanSyndromeDict.ToDictionary(k => k.Key, v =>
            {
                return Enumerable.Range(1, maxMonth).Select(item =>
                {
                    var faultMonthCount = v.Value.Where(c => Math.Ceiling(c.Key / 30) <= item && Math.Ceiling(c.Key / 30) > (item - 1)).Sum(c => c.Value);
                    return new MonthMetric()
                    {
                        Month = $"{item}个月",
                        TotalCount = faultMonthCount,
                    };
                }).ToList();
            });
            result.FaultSpanDistribution = syndromes.ToDictionary(k => k, v =>
            {
                return syndromeFaultMonthDict.ContainsKey(v) ? syndromeFaultMonthDict[v] : Enumerable.Range(1, maxMonth).Select(item =>
                {
                    return new MonthMetric()
                    {
                        Month = $"{item}个月",
                        TotalCount = 0,
                    };
                }).ToList();
            });
            #endregion

            #region 里程
            var mileAgeDict = dataList.GroupBy(x => (int)Math.Ceiling((double)x.MileAge / 5000)).OrderBy(x => x.Key).ToDictionary(k => k.Key, v => v.Count());
            var maxMile = mileAgeDict.Keys.Max();
            var minMile = mileAgeDict.Keys.Min();
            var mileSpan = maxMile - minMile + 1;
            result.MileAgeDistribution = Enumerable.Range(minMile, mileSpan)
                .ToDictionary(k => k * 5000, v =>
                {
                    return mileAgeDict.ContainsKey(v) ? mileAgeDict[v] : 0;
                });
            #endregion

            #region 特约店
            var allDealer = dataList.GroupBy(x => x.DealerName).OrderByDescending(x => x.Count()).ThenBy(x => x.Key);
            var top20Dealers = allDealer.Select(x => x.Key).Take(20);
            result.DealerDistribution = dataList.GroupBy(x => x.ModelYear)
                .ToDictionary(k => k.Key, v =>
                   {
                       var dealerDict = v.GroupBy(x => x.DealerName).ToDictionary(vk => vk.Key, vv => vv.Count());
                       var dealer = top20Dealers.ToDictionary(k => k, v => dealerDict.ContainsKey(v) ? dealerDict[v] : 0);
                       return dealer;
                   });

            #endregion

            #region 片区
            var provinceDict = dataList.Where(x => x.DataSource == DataSource.MQI).GroupBy(x => x.Features["所属省份"]).ToDictionary(k => k.Key, v => v.GroupBy(vx => vx.DealerName).ToDictionary(vk => vk.Key, vv => vv.Count()));

            var provinceList = ResourcesExtentions.GetAllResources(ProvinceSource.ResourceManager);
            result.RegionDistribution = provinceList.Select(x =>
            {
                var provinceValue = provinceDict.ContainsKey(x.Key) ? provinceDict[x.Key] : null;
                var regionMetric = new RegionMetric()
                {
                    Name = x.Value,
                    Value = provinceValue == null ? 0 : provinceValue.Sum(x => x.Value),
                    Top5Dealer = provinceValue?.OrderByDescending(x => x.Value).Take(5).ToDictionary(vk => vk.Key, vv => vv.Value)
                };
                return regionMetric;
            }).ToList();
            #endregion

            #region 经过月不良率
            result.DefectRateDistribution = new Dictionary<string, List<MonthRateMetric>>();
            var modelYearDict = dataList.GroupBy(x => x.ModelYear).OrderBy(x => x.Key).ToDictionary(k => k.Key, v => v.ToList());
            var sales_data_dict = all_sales_datas.GroupBy(x => x.ModelYear).ToDictionary(k => k.Key, v => v.ToList());
            foreach (var item in modelYearDict)
            {
                if (sales_data_dict.ContainsKey(item.Key))
                {
                    var sales_datas = sales_data_dict[item.Key];
                    var initDate = sales_datas.Select(x => x.InitialRegistDate).Min().Value;
                    var lastDay = item.Value.Max(x => x.FaultDate);
                    var monthSpan = MonthDifference(lastDay, initDate);
                    var defectRateDict = Enumerable.Range(0, monthSpan + 1).Select(monthItem =>
                    {
                        var lastMonthFirstDay = GetNextMonthFirstDay(initDate.AddMonths(monthItem));
                        var faultCount = item.Value.Count(x => x.FaultDate < lastMonthFirstDay);
                        var salesCount = sales_datas.LongCount(sale => sale.InitialRegistDate < lastMonthFirstDay);
                        return new MonthRateMetric()
                        {
                            Month = $"{monthItem + 1}个月",
                            DefectRate = (faultCount > 0 && salesCount > 0) ? (float)faultCount / salesCount : 0
                        };
                    }).ToList();
                    result.DefectRateDistribution.Add(item.Key, defectRateDict);
                }
            }
            #endregion

            #region 对策时间不良率
            result.DefectRateByCntrDistribution = new Dictionary<string, List<MonthRateByCntrMetric>>();

            foreach (var item in modelYearDict)
            {
                if (sales_data_dict.ContainsKey(item.Key))
                {
                    var sales_datas = sales_data_dict[item.Key];
                    var initDate = sales_datas.Select(x => x.InitialRegistDate).Min().Value;
                    var lastDay = item.Value.Max(x => x.FaultDate);
                    var monthSpan = MonthDifference(lastDay, initDate);
                    var monthCntrDict = item.Value.Where(x => x.RelatedInfo != null).SelectMany(c => c.RelatedInfo.Where(x => x.PermanentCntrTime.HasValue && x.PermanentCntrTime != DateTime.MaxValue).Select(x => x.PermanentCntrTime.Value))
                        .GroupBy(x=> MonthDifference(x, initDate)).ToDictionary(k=>k.Key,v=>v.OrderByDescending(t=>t).FirstOrDefault());

                    var tempFaultSpanByMonths = item.Value.GroupBy(x => MonthDifference(x.FaultDate, initDate)).ToDictionary(k => k.Key, v => v.ToList());
                    var tempMaxMonth = tempFaultSpanByMonths.Keys.Max();
                    DateTime? currentCntrTime = null;
                    DateTime? nextCntrTime = monthCntrDict.ContainsKey(0) ? monthCntrDict[0] : null;

                    var defectRateDict = Enumerable.Range(0, monthSpan + 1).Select(month =>
                      {
                          var lastMonthFirstDay = GetNextMonthFirstDay(initDate.AddMonths(month));
                          DateTime? cntr = monthCntrDict.ContainsKey(month) ? monthCntrDict[month] : null;
                         
                          var datasBeforeCntr = tempFaultSpanByMonths.Where(x => x.Key <= month).SelectMany(x => x.Value);

                          if (currentCntrTime.HasValue)
                          {
                              datasBeforeCntr = datasBeforeCntr.Where(x => x.ProductionDate > currentCntrTime.Value);
                          }
                          if (nextCntrTime.HasValue)
                          {
                              datasBeforeCntr = datasBeforeCntr.Where(x => x.ProductionDate <= nextCntrTime.Value);
                          }
                          //本月内对策前的发生件数
                          var countBeforeCntr = datasBeforeCntr.Count();
                          if (currentCntrTime.HasValue)
                              sales_datas = sales_datas.Where(sale => sale.ProductionDate > currentCntrTime).ToList();
                          var end = nextCntrTime ?? lastMonthFirstDay;
                          var salesCount = sales_datas.LongCount(sale => sale.ProductionDate < end);

                          if (cntr != null)
                          {
                              currentCntrTime = cntr.Value;
                              nextCntrTime = month < monthSpan && monthCntrDict.ContainsKey(month + 1) ? monthCntrDict[month + 1] : null;
                          }

                          return new MonthRateByCntrMetric()
                          {
                              Month = $"{month + 1}个月",
                              DefectRate = (countBeforeCntr > 0 && salesCount > 0) ? (float)countBeforeCntr / salesCount : 0f,
                              CntrTime = cntr
                          };

                      }).ToList();
                    result.DefectRateByCntrDistribution.Add(item.Key, defectRateDict);
                }
            }
            #endregion

            #region 渠道
            var ds_dataList = all_dataList.Where(x => x.DataSource != DataSource.MEDIA_MAIN).ToList();
            if (search.Datasource.Count > 0)
            {
                ds_dataList = ds_dataList.Where(x => search.Datasource.Contains(x.DataSource)).ToList();
            }

            result.DataSourceDistribution = new Dictionary<string, List<MonthMetric>>();
            
            var ds_minDate = ds_dataList.Select(x => x.FaultDate).Min();
            var ds_maxDate = ds_dataList.Select(x => x.FaultDate).Max();
            var faultByDataSourceDict = ds_dataList.GroupBy(x => ConvertDataSourceToType(x.DataSource)).ToDictionary(k => k.Key?.ToString(), v =>
            {
                var faultDistribution = v.GroupBy(x => x.FaultDate.ToString("yyyy-MM")).ToDictionary(k => k.Key, v => v.Count());
                var tempFault = ReConstruct(faultYears.Min(), faultYears.Max(), ds_minDate, ds_maxDate, faultDistribution);
                return tempFault;
            });
            var accumulateResult = AccumulateAllDataSource(faultYears.Min(), faultYears.Max(), faultByDataSourceDict);
            result.DataSourceDistribution = faultByDataSourceDict;
            result.DataSourceDistribution.Add("累计", accumulateResult);
            #endregion
            return result;
        }

        private bool DataSourceMatch(DataSource source, DataSourceType target)
        {
            switch (source)
            {
                case DataSource.GOV:
                    return target == DataSourceType.总局;
                case DataSource.HOTLINE:
                    return target == DataSourceType.热线800;
                case DataSource.MEDIA_SUB:
                    return target == DataSourceType.网络媒体;
                case DataSource.MQI:
                    return target == DataSourceType.MQI;
                case DataSource.TECH_CONSULTING:
                    return target == DataSourceType.技术咨询;
                default:
                    return false;
            }
        }

        private DataSourceType? ConvertDataSourceToType(DataSource source)
        {
            DataSourceType? result = null;
            switch (source)
            {
                case DataSource.GOV:
                    result = DataSourceType.总局;
                    break;
                case DataSource.HOTLINE:
                    result = DataSourceType.热线800;
                    break;
                case DataSource.MEDIA_SUB:
                    result = DataSourceType.网络媒体;
                    break;
                case DataSource.MQI:
                    result = DataSourceType.MQI;
                    break;
                case DataSource.TECH_CONSULTING:
                    result = DataSourceType.技术咨询;
                    break;
                default:
                    break ;
            }

            return result;
        }

        public async Task<List<DayMetric>> MetricsInOneMonth(BaseSearchModel search, string month, ChartType chartType)
        {
            var all_dataList = await AllDataSourceSearchAsync(search);
            var dataList = all_dataList;

            if (search.Datasource.Count > 0)
            {
                dataList = dataList.Where(x => search.Datasource.Contains(x.DataSource)).ToList();
            }

            if (chartType == ChartType.渠道)
            {
                dataList = dataList.Where(x => x.DataSource != DataSource.MEDIA_MAIN).ToList();
            }
            else
            {
                dataList = dataList.Where(x => x.DataSource != DataSource.MEDIA_SUB).ToList();
            }

            var results = new List<DayMetric>();
            if (chartType == ChartType.经过)
            {
                int.TryParse(Regex.Replace(month, @"[^0-9]+", ""), out int monthSpan);
                var daysKeyStart = monthSpan * 30;
                var daysSpanDict = dataList.GroupBy(x => (int)Math.Ceiling((x.FaultDate - x.InitialRegistDate).TotalDays)).Where(x => x.Key > daysKeyStart - 30 && x.Key <= daysKeyStart).ToDictionary(k => k.Key, v => v.Count());
                results = Enumerable.Range(daysKeyStart - 29, 30).Select(x => new DayMetric()
                {
                    Day = $"{x}日",
                    Count = daysSpanDict.ContainsKey(x) ? daysSpanDict[x] : 0
                }).ToList();
            }
            else
            {
                var monthDatas = new List<VehicleFaultListServiceModel>();
                /*计算当前月的天数*/
                var currentMonth = DateTime.Parse($"{month}-01");
                var nextMonth = currentMonth.AddMonths(1);
                var monthDays = (int)Math.Floor((nextMonth - currentMonth).TotalDays);
                //定义按天统计的字典，
                //key:"yyyy-MM-dd" 
                //value: 当天不良件数
                var daysDict = new Dictionary<string, int>();
                switch (chartType)
                {
                    case ChartType.生产:
                        monthDatas = dataList.Where(x => x.ProductionDate.ToString("yyyy-MM") == month).ToList();
                        daysDict = monthDatas.GroupBy(x => x.ProductionDate.ToString("yyyy-MM-dd")).ToDictionary(k => k.Key, v => v.Count());
                        break;
                    case ChartType.故障:
                        monthDatas = dataList.Where(x => x.FaultDate.ToString("yyyy-MM") == month).ToList();
                        daysDict = monthDatas.GroupBy(x => x.FaultDate.ToString("yyyy-MM-dd")).ToDictionary(k => k.Key, v => v.Count());
                        break;
                    case ChartType.渠道:
                        #region 其他各渠道
                        results = Enum.GetValues(typeof(DataSourceType)).Cast<DataSourceType>().SelectMany(item =>
                        {
                            var monthDatas = dataList.Where(x => DataSourceMatch(x.DataSource, item) && x.FaultDate.ToString("yyyy-MM") == month).ToList();
                            var daysDict = monthDatas.GroupBy(x => x.FaultDate.ToString("yyyy-MM-dd")).ToDictionary(k => k.Key, v => v.Count());
                            return Enumerable.Range(1, monthDays).Select(x =>
                            {
                                var day = new DateTime(currentMonth.Year, currentMonth.Month, x).Date.ToString("yyyy-MM-dd");
                                var count = daysDict.ContainsKey(day) ? daysDict[day] : 0;
                                return new DayMetric() { Day = day, Count = count, DataSource = item.ToString() };
                            }).ToList();
                        }).ToList();
                        #endregion

                        monthDatas = dataList.Where(x => x.FaultDate.ToString("yyyy-MM") == month).ToList();
                        daysDict = monthDatas.GroupBy(x => x.FaultDate.ToString("yyyy-MM-dd")).ToDictionary(k => k.Key, v => v.Count());
                        var allDays = Enumerable.Range(1, monthDays).Select(x =>
                        {
                            var day = new DateTime(currentMonth.Year, currentMonth.Month, x).Date.ToString("yyyy-MM-dd");
                            var count = daysDict.ContainsKey(day) ? daysDict[day] : 0;
                            return new DayMetric() { Day = day, Count = count, DataSource = "累计" };
                        }).ToList();
                        results.AddRange(allDays);
                        break;
                }

                if (chartType != ChartType.渠道)
                {
                    results = Enumerable.Range(1, monthDays).Select(x =>
                    {
                        var day = new DateTime(currentMonth.Year, currentMonth.Month, x).Date.ToString("yyyy-MM-dd");
                        var count = daysDict.ContainsKey(day) ? daysDict[day] : 0;
                        return new DayMetric() { Day = day, Count = count };
                    }).ToList();
                }
            }

            return results;
        }

        public async Task<Dictionary<string, List<DayRateMetric>>> CntrRateMetricsInOneMonth(BaseSearchModel search, string month)
        {
            var all_dataList = await GetAllDataSourceAsync(search);
            var dataList = all_dataList.Where(x=>x.DataSource != DataSource.MEDIA_SUB);
            if (search.Datasource.Count > 0)
            {
                dataList = all_dataList.Where(x => search.Datasource.Contains(x.DataSource)).ToList();
            }
            var defectRateByCntrDistribution = new Dictionary<string, List<DayRateMetric>>();
            var modelYearDict = dataList.GroupBy(x => x.ModelYear).ToDictionary(k => k.Key, v => v.ToList());

            foreach (var item in modelYearDict)
            {
                var sales_datas = await rawSaleService.GetSalesDatas(search.CarModel, search.CarType, new List<string> { item.Key }, null, null, null);
                var initDate = sales_datas.Select(x => x.InitialRegistDate).Min().Value;
                int.TryParse(Regex.Replace(month, @"[^0-9]+", ""), out int monthSpan);

                var monthCntrDict = item.Value.GroupBy(x => MonthDifference(x.FaultDate, initDate)).Select(x =>
                {
                    var cntrs = x.Where(x => x.RelatedInfo != null).SelectMany(c => c.RelatedInfo.Where(x => x.PermanentCntrTime != null && x.PermanentCntrTime != DateTime.MaxValue).Select(x => x.PermanentCntrTime.Value)).ToList();
                    return new KeyValuePair<int, List<DateTime>>(x.Key, cntrs.Count > 0 ? cntrs : null);
                }).Where(x => x.Value != null).GroupBy(x => x.Key).ToDictionary(k => k.Key, v => v.SelectMany(x => x.Value).Distinct().OrderByDescending(x => x).FirstOrDefault());

                DateTime? previousCntrTime = monthCntrDict.Where(x => x.Key < monthSpan).OrderBy(x => x.Key).Select(x => x.Value).LastOrDefault();
                DateTime? nextCntrTime = monthCntrDict.ContainsKey(monthSpan) ? monthCntrDict[monthSpan] : null;

                var selectDate = initDate.AddMonths(monthSpan - 1);
                var firstDayOfSelectMonth = new DateTime(selectDate.Year, selectDate.Month, 1);
                var selectMonthDays = firstDayOfSelectMonth.AddMonths(1).AddDays(-1).Day;

                var itemDays = Enumerable.Range(1, selectMonthDays).Select(async x =>
                {
                    var currentDay = firstDayOfSelectMonth.AddDays(x);

                    #region 获取销量
                    if (previousCntrTime.HasValue)
                        sales_datas = sales_datas.Where(sale => sale.ProductionDate > previousCntrTime).ToList();
                    var end = nextCntrTime ?? currentDay;
                    var salesCount = sales_datas.LongCount(sale => sale.ProductionDate< end);
                    #endregion

                    var datasBeforeCurrentDay = item.Value.Where(t => t.FaultDate < currentDay);
                    if (previousCntrTime.HasValue)
                    {
                        datasBeforeCurrentDay = datasBeforeCurrentDay.Where(c => c.ProductionDate > previousCntrTime.Value);
                    }

                    if (nextCntrTime.HasValue && nextCntrTime.Value.Date == currentDay)
                    {
                        datasBeforeCurrentDay = datasBeforeCurrentDay.Where(x => x.ProductionDate <= nextCntrTime.Value);
                        previousCntrTime = nextCntrTime;
                        nextCntrTime = null;
                    }

                    var countBeforeCurrentDay = datasBeforeCurrentDay.Count();

                    var dayMetric = new DayRateMetric()
                    {
                        Day = $"{x}日",
                        DefectRate = salesCount > 0 ? (float)countBeforeCurrentDay / salesCount : 0
                    };
                    return dayMetric;
                }).Select(x=>x.Result).ToList();
                defectRateByCntrDistribution.Add(item.Key, itemDays);
            }

            return defectRateByCntrDistribution;
        }

        public async Task<Dictionary<string, List<DayRateMetric>>> RateMetricsInOneMonth(BaseSearchModel search, string month)
        {
            var all_dataList = await AllDataSourceSearchAsync(search);
            var dataList = all_dataList.Where(x => x.DataSource != DataSource.MEDIA_SUB);
            if (search.Datasource.Count > 0)
            {
                dataList = all_dataList.Where(x => search.Datasource.Contains(x.DataSource)).ToList();
            }
            var defectRateDistribution = new Dictionary<string, List<DayRateMetric>>();
            var modelYearDict = dataList.GroupBy(x => x.ModelYear).ToDictionary(k => k.Key, v => v.ToList());
            foreach (var item in modelYearDict)
            {
                var sales_datas = await rawSaleService.GetSalesDatas(search.CarModel, search.CarType, new List<string> { item.Key }, null, null, null);
                var initDate = sales_datas.Select(x => x.InitialRegistDate).Min().Value;
                int.TryParse(Regex.Replace(month, @"[^0-9]+", ""), out int monthSpan);

                var selectDate = initDate.AddMonths(monthSpan - 1);
                var firstDayOfSelectMonth = new DateTime(selectDate.Year, selectDate.Month, 1);
                var selectMonthDays = firstDayOfSelectMonth.AddMonths(1).AddDays(-1).Day;

                var itemDays = Enumerable.Range(1, selectMonthDays).Select(x =>
                {
                    var currentDay = firstDayOfSelectMonth.AddDays(x);
                    #region 获取销量
                    var salesCount = sales_datas.LongCount(sale => sale.InitialRegistDate < currentDay);
                    #endregion
                    var countBeforeCurrentDay = item.Value.Count(t => t.FaultDate <= currentDay);
                    var dayMetric = new DayRateMetric()
                    {
                        Day = $"{x}日",
                        DefectRate = salesCount > 0 ? (float)countBeforeCurrentDay / salesCount : 0
                    };
                    return dayMetric;
                }).ToList();
                defectRateDistribution.Add(item.Key, itemDays);
            }
            return defectRateDistribution;
        }

        private List<MonthMetric> ReConstruct(int minYear, int maxYear, DateTime minDate, DateTime maxDate, Dictionary<string, int> metrics)
        {
            var yearCount = (maxYear - minYear) + 1;
            var result = Enumerable.Range(minYear, yearCount)
                .ToList().SelectMany(year =>
                {
                    var startMonth = 1;
                    var endMonth = 12;
                    if (year == minYear)
                        startMonth = minDate.Month;
                    else if (year == maxYear)
                        endMonth = maxDate.Month;
                    var monthCount = endMonth - startMonth + 1;

                    return Enumerable.Range(startMonth, monthCount).Select(month =>
                    {
                        var firstDayOfMonth = new DateTime(year, month, 1);
                        var nextDayOfMonth = firstDayOfMonth.AddMonths(1);
                        var monthDays = (int)Math.Floor((nextDayOfMonth - firstDayOfMonth).TotalDays);
                        var tempKey = firstDayOfMonth.ToString("yyyy-MM");
                        return new MonthMetric()
                        {
                            Month = tempKey,
                            TotalCount = metrics.ContainsKey(tempKey) ? metrics[tempKey] : 0
                        };
                    }).ToList();
                }).ToList();

            return result;
        }

        private List<MonthMetric> AccumulateAllDataSource(int minYear, int maxYear, Dictionary<string, List<MonthMetric>> metrics)
        {
            var yearCount = (maxYear - minYear) + 1;
            var allMetrics = metrics.Values.SelectMany(x => x);
            var allMonth = allMetrics.Select(x => x.Month).OrderBy(x => x).Distinct();
            DateTime.TryParse($"{allMonth.Min()}-01", out DateTime minDate);
            DateTime.TryParse($"{allMonth.Max()}-01", out DateTime maxDate);
            var result = Enumerable.Range(minYear, yearCount)
                .SelectMany(year =>
                {
                    var startMonth = 1;
                    var endMonth = 12;
                    if (year == minYear)
                        startMonth = minDate.Month;
                    else if (year == maxYear)
                        endMonth = maxDate.Month;
                    var monthCount = endMonth - startMonth + 1;

                    return Enumerable.Range(startMonth, monthCount).Select(month =>
                    {
                        var firstDayOfMonth = new DateTime(year, month, 1);
                        var nextDayOfMonth = firstDayOfMonth.AddMonths(1);
                        var monthDays = (int)Math.Floor((nextDayOfMonth - firstDayOfMonth).TotalDays);
                        var tempKey = firstDayOfMonth.ToString("yyyy-MM");
                        var totalCount = allMetrics.Where(a => a.Month == tempKey).Sum(a => a.TotalCount);
                        return new MonthMetric()
                        {
                            Month = tempKey,
                            TotalCount = totalCount
                        };
                    }).ToList();
                }).ToList();

            return result;
        }

        public async Task<List<string>> GetAllCarTypes()
        {
            var dataList = await AllDataSourceSearchAsync(new BaseSearchModel());
            var carTypes = dataList.Select(x => x.CarType).Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            return carTypes;
        }

        public async Task<List<CircularChartMetrics<SyndromeMetric>>> GetSyndromeMetrics(BaseSearchModel search)
        {
            var dataList = await AllDataSourceSearchAsync(search);
            
            #region 症状
            var result = dataList.Where(x => !string.IsNullOrWhiteSpace(x.Syndrome))
                .GroupBy(x => x.CarModel)
                .Select(k => new CircularChartMetrics<SyndromeMetric>()
                {
                    CarModel = k.Key,
                    TotalCount = k.Count(),
                    Metrics = k.GroupBy(k => k.SyndromeModel)
                    .Select(m => {
                        var riskLevel = warningRecordService.GetRiskLevel(new BaseSearchModel(search.CarModel, search.CarType, search.YearModels, search.PartName, new List<string>() { m.Key.Id }, null), WarningUnit.零件_不良症状).Result;
                        return new SyndromeMetric()
                        {
                            Id = m.Key.Id,
                            Name = m.Key.Name,
                            RiskLevel = $"风险：{riskLevel.Item1?.ToString() ?? ConstantSettings.UnknownString}，{riskLevel.Item2?.ToString() ?? ConstantSettings.UnknownString}",
                            TotalCount = m.Count()
                        };
                    }).ToList()
                }).ToList();
            #endregion
            return result;
        }

        public async Task<List<PartNameMetric>> GetPartNameMetrics(BaseSearchModel search)
        {
            var dataList = await AllDataSourceSearchAsync(search);
            #region 零件名
            var result = dataList.GroupBy(x => x.PartName)
                .Select(k => new PartNameMetric()
                {
                    Name = k.Key,
                    TotalCount = k.Count()
                }).ToList();
            #endregion
            return result;
        }

        /// <summary>
        /// 获取所有渠道的符合条件的数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private async Task<List<VehicleFaultListServiceModel>> AllDataSourceSearchAsync(BaseSearchModel search)
        {
            return await vehicleFaultDataService.ListStatisticableAsync(search.CarModel, null, search.CarType,
                search.YearModels, search.PartName, search.Syndrome, null, null);
        }

        private async Task<List<VehicleFaultServiceModel>> GetAllDataSourceAsync(BaseSearchModel search)
        {
            return await vehicleFaultDataService.ListExportableAsync(search.CarModel, null, search.CarType,
                search.YearModels, search.PartName, search.Syndrome, null, null);
        }

        private int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        public DateTime GetNextMonthFirstDay(DateTime dateTime)
        {
            DateTime d1 = new DateTime(dateTime.Year, dateTime.Month, 1);
            DateTime d2 = d1.AddMonths(1);
            return d2;
        }
    }
}

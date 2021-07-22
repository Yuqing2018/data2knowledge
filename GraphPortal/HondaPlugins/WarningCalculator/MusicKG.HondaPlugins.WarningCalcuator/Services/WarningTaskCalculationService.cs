using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.Services.Models;
using MusicKG.HondaPlugins.Services.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.WarningCalculator.Services
{
    public class WarningTaskCalculationService : IWarningTaskCalculationService
    {
        private readonly IRawSalesService rawSaleService;
        private readonly IWarningRecordService warningRecordsService;
        private readonly ILogger<WarningTaskCalculationService> logger;

        public WarningTaskCalculationService(
            IRawSalesService rawSaleService, 
            IWarningRecordService warningRecordsService,
            ILogger<WarningTaskCalculationService> logger)
        {
            this.rawSaleService = rawSaleService;
            this.warningRecordsService = warningRecordsService;
            this.logger = logger;
        }

        public async Task<List<WarningRecordDataModel>> CalculateWarningAsync(string taskId, DateTime currentRunTime, WarningTaskDataModel task, List<VehicleFaultServiceModel> data)
        {
            //总销量
            List<RawSalesDataServiceModel> salesDatas = new List<RawSalesDataServiceModel>();

            long salesCount = 0;

            Dictionary<DataSource, (int carTypeCount, int lastThreeCarTypeCount)> ds_carTypeDict = new Dictionary<DataSource, (int carTypeCount, int lastThreeCarTypeCount)>();

            List<WarningRecordDataModel> all_reocrds = new List<WarningRecordDataModel>();

            var calculateAgain = task.WarningIndex.Any(w => w.WarningType == WarningType.再发预警);
            var calculateRisk = task.WarningIndex.Any(w => w.WarningType == WarningType.风险预警);
            var calculateMuti = task.WarningIndex.Any(w => w.WarningType == WarningType.多发预警);
            
            switch (task.WarningUnit)
            {
                case WarningUnit.零件_不良症状:
                    data = data.Where(x => x.PartName != ConstantSettings.UnknownString).ToList();
                    break;
                case WarningUnit.未知零件_不良症状:
                    data = data.Where(x => x.PartName == ConstantSettings.UnknownString).ToList();
                    break;
                case WarningUnit.不良症状:
                default:
                    break;
            }

            if (calculateAgain)
            {
                salesDatas = await rawSaleService.GetSalesDatas(task.CarModels, task.CarTypes, task.YearModels, null, null, currentRunTime);
                salesCount = salesDatas?.LongCount() ?? 0;
                logger.LogInformation($"Get {salesDatas.Count} sales data for again.");
            }
            
            if (calculateMuti || calculateRisk)
            {
                salesCount = salesCount > 0 ? salesCount : await rawSaleService.GetSalesCount(task.CarModels, task.CarTypes, task.YearModels, null, null, currentRunTime);
                logger.LogInformation($"Get sales data count {salesCount} for multi and risk.");
                ds_carTypeDict = GetLastThreeCarTypeCountByDataSource(data, currentRunTime);

                if (calculateRisk)
                {
                    all_reocrds = await warningRecordsService.GetTaskWarningRecords(taskId);
                    logger.LogInformation($"Get {all_reocrds.Count} records for risk warning.");
                }
            }

            var records = new List<WarningRecordDataModel>();
            if (task.WarningUnit != WarningUnit.不良症状)
            {
                //按照零件号零件名分组
                records = data.GroupBy(x => new { x.PartName, x.SyndromeModel }).Select(b =>
                   {
                       logger.LogInformation($"Start calculating warning for PartName: {b.Key.PartName}, Syndrome: {b.Key.SyndromeModel.Id} - {b.Key.SyndromeModel.Name}");
                       var multiDataSourceDict = calculateMuti ? GetMultipleWarningMetrics(b.ToList(), salesCount, ds_carTypeDict, currentRunTime) : new Dictionary<string, MultipleWarningIndexMetricsDataModel>();
                       var record = new WarningRecordDataModel()
                       {
                           Id = ObjectId.GenerateNewId(),
                           TaskId = taskId,
                           PartNo = b.FirstOrDefault()?.PartNo,
                           PartName = b.Key.PartName,
                           Syndrome = b.Key.SyndromeModel.Id,
                           SyndromeBadGrade = b.Key.SyndromeModel.BadGrade,
                           WarningTime = DateTime.UtcNow,
                           MultipleMetrics = multiDataSourceDict,
                           RiskMetrics = calculateRisk ? GetRiskWarningMetrics(taskId, task.WarningUnit, b.ToList(), salesCount, ds_carTypeDict, all_reocrds, currentRunTime) : new RiskWarningIndexMetricsDataModel { DataSourceMetrics = new Dictionary<string, DataSourceMetricsDataModel>() },
                           AgainMetrics = calculateAgain ? GetAgainWarningMetrics(b.ToList(), salesDatas) : new AgainWarningIndexMetricsDataModel()
                       };
                       WetherOccurs(task, record);
                       logger.LogInformation($"Finish calculating warning for PartName: {b.Key.PartName}, Syndrome: {b.Key.SyndromeModel.Id} - {b.Key.SyndromeModel.Name}");
                       return record;
                   }).ToList();
            }
            else
            {
                records = data.GroupBy(x => x.SyndromeModel).Select(b =>
                {
                    logger.LogInformation($"Start calculating warning for Syndrome: {b.Key.Id} - {b.Key.Name}");
                    var multiDataSourceDict = calculateMuti ? GetMultipleWarningMetrics(b.ToList(), salesCount, ds_carTypeDict, currentRunTime) : new Dictionary<string, MultipleWarningIndexMetricsDataModel>();
                    var record = new WarningRecordDataModel()
                    {
                        Id = ObjectId.GenerateNewId(),
                        TaskId = taskId,
                        PartNo = "",
                        PartName = "",
                        Syndrome = b.Key.Id,
                        WarningTime = DateTime.UtcNow,
                        MultipleMetrics = multiDataSourceDict,
                        RiskMetrics = calculateRisk ? GetRiskWarningMetrics(taskId, task.WarningUnit, b.ToList(), salesCount, ds_carTypeDict, all_reocrds, currentRunTime) : new RiskWarningIndexMetricsDataModel { DataSourceMetrics = new Dictionary<string, DataSourceMetricsDataModel>() },
                        AgainMetrics = calculateAgain ? GetAgainWarningMetrics(b.ToList(), salesDatas) : new AgainWarningIndexMetricsDataModel()
                    };
                    WetherOccurs(task, record);
                    logger.LogInformation($"Finish calculating warning for Syndrome: {b.Key.Id} - {b.Key.Name}");
                    return record;
                }).ToList();
            }

            if (calculateMuti)
            {
                logger.LogInformation($"Start calculating top for muti-warning.");
                Enum.GetValues(typeof(DataSource)).Cast<DataSource>().ToList().ForEach(item =>
                {
                    var itemOrderDict = records.GroupBy(x => x.MultipleMetrics.ContainsKey(item.ToString()) ? x.MultipleMetrics[item.ToString()].TotalCount : 0)
                        .OrderByDescending(x => x.Key).ToDictionary(k => k.Key, v => v.Select(x => x.Id).ToList());

                    var itemOders = itemOrderDict.Keys.ToList();
                    var currentSourceTopOrder = new Dictionary<ObjectId, int>();
                    foreach (var dict in itemOrderDict)
                    {
                        dict.Value.ForEach(obj =>
                        {
                            currentSourceTopOrder.Add(obj, itemOders.IndexOf(dict.Key));
                        });
                    }

                    records.ForEach(record =>
                      {
                          if (record.MultipleMetrics.ContainsKey(item.ToString()))
                          {
                              record.MultipleMetrics[item.ToString()].TopOrder = currentSourceTopOrder[record.Id];
                          }
                      });
                });
                logger.LogInformation($"Finish calculating top for muti-warning.");
            }
            return records;
        }

        private Dictionary<DataSource,(int carTypeCount, int lastThreeCarTypeCount)> GetLastThreeCarTypeCountByDataSource(List<VehicleFaultServiceModel> data, DateTime currentRunTime)
        {
            var ds_carTypeDict = data.GroupBy(x => x.DataSource).ToDictionary(k => k.Key, v =>
            {
                //所选车型发生总件数
                var carTypeCount = v.Count();

                //三个月前第一天日期
                var currentMonthFirstDate = currentRunTime.AddMonths(-3);

                //所选车型近三月的发生件数
                var lastThreeCarTypeMonthCount = v.Count(x=>x.FaultDate >= currentMonthFirstDate);
                return (carTypeCount, lastThreeCarTypeMonthCount);
            });

            return ds_carTypeDict;
        }

        /// <summary>
        /// 多发预警指标计算
        /// </summary>
        /// <param name="dataList">固定某车款，车型，年款,零件,不良症状的列表</param>
        /// <param name="warningIndex">预警指标</param>
        /// <param name="salesCount">车款，车型，年款销量</param>
        /// <param name="carTypeCount">当前车型下不良件数</param>
        /// <param name="lastThreeCarTypeMonthCount">当前车型近三个月的不良件数</param>
        /// <returns>分渠道的多发预警指标计算结果</returns>
        private Dictionary<string, MultipleWarningIndexMetricsDataModel> GetMultipleWarningMetrics(List<VehicleFaultServiceModel> dataList, long salesCount, Dictionary<DataSource,(int carTypeCount, int lastThreeCarTypeMonthCount)> ds_carTypeDict, DateTime currentRunTime)
        {
            var warningMetrics = dataList.GroupBy(x => x.DataSource).ToDictionary(k => k.Key.ToString(), b =>
            {
                //按年款区分的不良件数统计
                var countByYear = b.GroupBy(m => m.ModelYear).OrderByDescending(x => x.Key).ToDictionary(k => k.Key, v => v.Count());

                var totalCount = b.Count();

                var lastThree = Enumerable.Range(1, 3).Select(i => {
                    var firstDay = currentRunTime.AddMonths(-i);
                    var lastDay = currentRunTime.AddMonths(1 - i);
                    return new
                    {
                        month = $"第{i}月",
                        count = b.Count(x=>x.FaultDate > firstDay && x.FaultDate <= lastDay)
                    };
                }).ToDictionary(k => k.month, v => v.count);

                var lastThreeCount = lastThree.Sum(x => x.Value);
                //零件近3月 / 零件近3月前所有
                var partRate = (totalCount == lastThreeCount) ? 0 : (float)lastThreeCount / (totalCount - lastThreeCount);

                //车型近3月 / 车型近3月前所有
                var carTypeRate = 0d;
                if (ds_carTypeDict.ContainsKey(b.Key))
                {
                    //该渠道下所选车型的总不良件量
                    var carTypeCount = ds_carTypeDict[b.Key].carTypeCount;
                    //该渠道下所选车型近三月不良件量
                    var lastThreeCarTypeMonthCount = ds_carTypeDict[b.Key].lastThreeCarTypeMonthCount;

                    carTypeRate = carTypeCount == lastThreeCarTypeMonthCount ? 0 : (float)lastThreeCarTypeMonthCount / (carTypeCount - lastThreeCarTypeMonthCount);
                }
               
                //多发预警每个不良症状的指标计算
                var warningIndex = new MultipleWarningIndexMetricsDataModel()
                {
                    TotalCount = totalCount,
                    LastThreeMonthCount = lastThree,
                    DefectRate = salesCount > 0 ? (float)totalCount / salesCount : 0,
                    LastThreeMonthAscentRate = carTypeRate == 0 ? 0 : partRate / carTypeRate,
                    DefectRateByYearModel = countByYear.ToDictionary(x => x.Key, v => salesCount > 0 ? (double)v.Value / salesCount : 0d)
                };
                return warningIndex;
            });
            return warningMetrics;
        }

        /// <summary>
        /// 风险预警指标计算
        /// </summary>
        /// <param name="dataList">固定某车款，车型，年款,零件,不良症状的列表</param>
        /// <param name="warningIndex">预警指标</param>
        /// <param name="salesCount">车款，车型，年款销量</param>
        /// <param name="carTypeCount">当前车型下不良件数</param>
        /// <param name="lastThreeCarTypeMonthCount">当前车型近三个月的不良件数</param>
        /// <returns></returns>
        private RiskWarningIndexMetricsDataModel GetRiskWarningMetrics(string taskId, WarningUnit warningUnit, List<VehicleFaultServiceModel> dataList, long salesCount, Dictionary<DataSource, (int carTypeCount, int lastThreeCarTypeMonthCount)> ds_carTypeDict, List<WarningRecordDataModel> allRecords, DateTime currentRunTime)
        { 
            var carModels = dataList.Select(x => x.CarModel).Distinct().ToList();
            var syndromeBadGrade = dataList.Select(x => x.SyndromeModel.BadGrade).FirstOrDefault();
            var fee = 0d;
            var meanCostRepair = 0d;
            //不良症状分渠道统计
            var datasourceDict = dataList.GroupBy(x => x.DataSource).ToDictionary(k => k.Key.ToString(), v =>
            {
                var totalCount = v.Count();
                
                //计算保修金额,只限定在MQI渠道
                if (v.Key == DataSource.MQI)
                {
                    var carModels = v.Select(x => x.CarModel).Distinct().ToList();
                    if (carModels.All(c => ConstantSettings.CarModelsOdBrandA.Contains(c)))
                    {
                        fee = v.Sum(x => x.CostRepair);
                        meanCostRepair = fee / totalCount;
                    }
                    else
                    {
                        var brandHdatas = v.Where(x => !ConstantSettings.CarModelsOdBrandA.Contains(x.CarModel)).ToList();
                        fee = brandHdatas.Sum(x => x.CostRepair);

                        meanCostRepair = fee / brandHdatas.Count;
                    }
                }
               
                var monthDict = v.GroupBy(m => m.FaultDate.ToString("yyyy-MM")).OrderByDescending(x => x.Key).ToDictionary(k => k.Key, v => v.Count());

                //三月前第一天日期
                var currentMonthFirstDate = currentRunTime.AddMonths(-3);
                //三个月前的不良件数
                var lastThreeBeforeCount = v.Count(x => x.FaultDate <= currentMonthFirstDate);
                //近三个月的不良件数
                var lastThreeCount = v.Count(x => x.FaultDate > currentMonthFirstDate);

                //零件近3月 / 零件近3月前所有
                var partRate = (lastThreeBeforeCount == 0) ? 0 : (float)lastThreeCount / lastThreeBeforeCount;

                //车型近3月 / 车型近3月前所有
                var carTypeRate = 0d;
                if (ds_carTypeDict.ContainsKey(v.Key))
                {
                    //该渠道下所选车型的总不良件量
                    var carTypeCount = ds_carTypeDict[v.Key].carTypeCount;
                    //该渠道下所选车型近三月不良件量
                    var lastThreeCarTypeMonthCount = ds_carTypeDict[v.Key].lastThreeCarTypeMonthCount;

                    carTypeRate = carTypeCount == lastThreeCarTypeMonthCount ? 0 : (float)lastThreeCarTypeMonthCount / (carTypeCount - lastThreeCarTypeMonthCount);
                }

                return new DataSourceMetricsDataModel
                {
                    TotalCount = v.Count(),
                    DefectRate = salesCount > 0 ? (float)v.Count() / salesCount : 0,
                    LastThreeMonthAscentRate = carTypeRate == 0 ? 0 : partRate / carTypeRate,
                };
            });

            //最新对策情况
            var pushSituation = dataList.SelectMany(x => x.RelatedInfo ?? new List<VehicleFaultRelatedDataModel>()).OrderBy(x => x.PermanentCntrTime).LastOrDefault();

            #region 推进状态 设置
            PushStatus? push_status = null;
            var first = dataList.FirstOrDefault();
            var lastConfirmRecord = GetLastConfirmRecord(allRecords,
                warningUnit == WarningUnit.不良症状 ? "" : first.PartName,
                first.Syndrome);

            if (lastConfirmRecord != null)
                push_status = lastConfirmRecord.PushStatus;
            else if (pushSituation?.PermanentCntrTime != null)
                push_status = PushStatus.完了;
            else if (dataList.Any(x => x.RelatedInfo.Any(a => a.QISNo != null || a.QICNo != null)))
                push_status = PushStatus.推进中;
            else
                push_status = PushStatus.未立项;
            #endregion

            var warningMetrics = new RiskWarningIndexMetricsDataModel
            {
                DataSourceMetrics = datasourceDict,
                PushStatus = push_status,
                PermanentCntr = pushSituation?.PermanentCntr,
                PermanentCntrTime = pushSituation?.PermanentCntrTime,
                CntrMesrReasonDesc = pushSituation?.CntrMesrReasonDesc,
                MeanCostRepair = meanCostRepair,
                IsExcessive = lastConfirmRecord?.IsExcessive,
                PermanentCntrStatus = lastConfirmRecord?.PermanentCntrStatus,
                AIRiskLevel = null,
                UsedForModel = false,
            };

            var otherMetrics = datasourceDict.Where(x => x.Key != DataSource.MQI.ToString()).Select(x => x.Value).ToList();
            var (totalCount, lastThreeAscentRate) = (otherMetrics.Sum(x => x.TotalCount), otherMetrics.Sum(x => x.LastThreeMonthAscentRate));
            /*风险得分 =（分值不良等级* 权重不良等级0.9+分值件数 * 权重件数0.5 + 分值上升率 * 权重上升率0.5 + 分值推进分类 * 权重推进分类0.1）*系数保修金额 * 系数超标影响 * 系数对策状态
            0.9 * [不良等级得分] + 【mqi】0.5*[不良件数或者不良率得分] + 0.5 *[近3月上升率] + 【其他渠道】0.1*[件数]+0.1*[近3月上升率] + 0.1*[推进分类]*/
            var score = 0.9 * GetGradeScore(syndromeBadGrade)
                  + 0.5 * GetMQIScore(datasourceDict.ContainsKey(DataSource.MQI.ToString())? datasourceDict[DataSource.MQI.ToString()]:null)
                  + 0.1 * GetOtherScore(totalCount, lastThreeAscentRate)
                  + 0.1 * GetPushScore(warningMetrics.PushStatus);
            //附加系数1：保修金额
            var coefficient1 = GetCoefficient(carModels, meanCostRepair);
            //附加系数1：超标影响
            var coefficient2 = warningMetrics.IsExcessive.HasValue && warningMetrics.IsExcessive.Value ? 1.1 : 1;
            //附加系数3：对策状态
            var coefficient3 = GetCoefficient(warningMetrics.PermanentCntrStatus);
            //计算风险得分
            warningMetrics.RiskScore = score * coefficient1 * coefficient2 * coefficient3;
            //计算风险等级
            warningMetrics.RiskLevel = SetRiskLevel(warningMetrics.RiskScore);
            return warningMetrics;
        }

        private WarningConfirmRecordDataModel GetLastConfirmRecord(List<WarningRecordDataModel> allRecords, string partName, string syndrome)
        {
            var querable = allRecords?.Where(x => x.IsMultipleWarning || x.IsRiskWarning || x.IsAgainWarning);

            if (!string.IsNullOrWhiteSpace(partName))
            {
                querable = querable?.Where(x => x.PartName == partName);
            }
            if (!string.IsNullOrWhiteSpace(syndrome))
            {
                querable = querable?.Where(x => x.Syndrome == syndrome);
            }
            var last_record = querable?.Where(x => x.ConfirmRecord != null)?
                .OrderByDescending(x => x.WarningTime)?
                .ThenByDescending(x => x.ConfirmRecord.LastConfirmdTime)?
                .Select(x => x.ConfirmRecord)?.FirstOrDefault();

            logger.LogInformation($"Get Confirm Record '{last_record?.ConfirmedMessage}' for risk warning.");

            return last_record;
        }

        /// <summary>
        /// 获取再发预警指标
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="salesDatas"></param>
        /// <returns></returns>
        private AgainWarningIndexMetricsDataModel GetAgainWarningMetrics(List<VehicleFaultServiceModel> dataList, List<RawSalesDataServiceModel> salesDatas)
        {
            //最新对策
            var pushSituation = dataList.SelectMany(x => x.RelatedInfo ?? new List<VehicleFaultRelatedDataModel>()).OrderBy(x => x.PermanentCntrTime).LastOrDefault();
            var cntrDataList = dataList.Where(x => x.DataSource == DataSource.MQI).ToList();
            //计算对策后不良件数
            var countAfterCntr = pushSituation != null ? cntrDataList.Count(x => x.ProductionDate > pushSituation.PermanentCntrTime) : 0;
            //计算对策前不良件数
            var countBeforeCntr = pushSituation != null ? cntrDataList.Count(x => x.ProductionDate <= pushSituation.PermanentCntrTime) : 0;
            //对策前销量初始化
            var salesBeforeCntrCount = (long)salesDatas.Count;
            //对策后销量初始化
            var salesAfterCntrCount = (long)salesDatas.Count;
            //根据对策时间调整
            if (pushSituation?.PermanentCntrTime != null)
            {
                salesBeforeCntrCount = salesDatas.LongCount(x => x.ProductionDate < pushSituation?.PermanentCntrTime);
                salesAfterCntrCount = salesDatas.LongCount(x => x.ProductionDate >= pushSituation?.PermanentCntrTime);
            }

            var warningMetric = new AgainWarningIndexMetricsDataModel()
            {
                PermanentCntr = pushSituation?.PermanentCntr,
                PermanentCntrTime = pushSituation?.PermanentCntrTime,
                CntrMesrReasonDesc = pushSituation?.CntrMesrReasonDesc,
                CountAfterCntr = countAfterCntr,
                DefectRateBeforeCntr = salesBeforeCntrCount == 0 ? 0d : (float)countBeforeCntr / salesBeforeCntrCount,
                DefectRateAfterCntr = salesAfterCntrCount == 0 ? 0d : (float)countAfterCntr / salesAfterCntrCount
            };
            return warningMetric;
        }

        private double GetCoefficient(PermanentCntrStatus? permanentCntrStatus)
        {
            var result = permanentCntrStatus switch
            {
                PermanentCntrStatus.未明确 => 1.2,
                PermanentCntrStatus.已明确 => 1.1,
                _ => 1,
            };
            return result;
        }

        private double GetCoefficient(List<string> carModels, double fee)
        {
            double result;
            if (carModels.All(c => ConstantSettings.CarModelsOdBrandA.Contains(c)))
            {
                if (fee >= 20000)
                    result = 1.15;
                else if (fee >= 10000)
                    result = 1.1;
                else if (fee >= 5000)
                    result = 1.05;
                else
                    result = 1;
            }
            else
            {
                if (fee >= 10000)
                    result = 1.15;
                else if (fee >= 5000)
                    result = 1.1;
                else if (fee >= 2000)
                    result = 1.05;
                else
                    result = 1;
            }

            return result;
        }

        private int GetMQIScore(DataSourceMetricsDataModel metric)
        {
            var score = 0;

            if (metric == null)
            {
                score = 1;
            }
            else
            {
                if (metric.TotalCount >= 250 || metric.DefectRate >= 0.001)
                {
                    score += 5;
                }
                else if (metric.TotalCount >= 125 || metric.DefectRate >= 0.0005)
                {
                    score += 3;
                }
                else
                    score += 1;

                if (metric.LastThreeMonthAscentRate >= 8)
                    score += 5;
                else if (metric.LastThreeMonthAscentRate >= 5)
                    score += 3;
                else if (metric.LastThreeMonthAscentRate >= 2)
                    score += 1;
            }

            return score;
        }

        private int GetOtherScore(int totalCount, double lastThreeMonthAscentRate)
        {
            int score = 0;

            if (totalCount >= 5)
                score += 5;
            else if (totalCount >= 2)
                score += 3;
            else if (totalCount >= 1)
                score += 1;

            if (lastThreeMonthAscentRate >= 8)
                score += 5;
            else if (lastThreeMonthAscentRate >= 5)
                score += 3;
            else if (lastThreeMonthAscentRate >= 2)
                score += 1;

            return score;
        }

        private int GetGradeScore(BadGrade grade)
        {
            var score = 0;
            switch (grade)
            {
                case BadGrade.A:
                    score = 5;
                    break;
                case BadGrade.B:
                    score = 3;
                    break;
                case BadGrade.C:
                    score = 1;
                    break;
            }
            return score;
        }

        private int GetPushScore(PushStatus? pushStatus)
        {
            var score = 0;
            switch (pushStatus)
            {
                case PushStatus.未立项:
                case PushStatus.需再立项:
                    score = 5;
                    break;
                case PushStatus.推进中:
                    score = 3;
                    break;
                case PushStatus.完了:
                case PushStatus.监视:
                    score = 1;
                    break;
                default:
                    break;
            }
            return score;
        }

        private RiskLevel SetRiskLevel(double score)
        {
            if (score >= 9)
                return RiskLevel.重点关注;
            else if (score >= 7)
                return RiskLevel.高风险;
            else if (score >= 6)
                return RiskLevel.潜在高风险;
            else if (score >= 5)
                return RiskLevel.中风险;
            else if (score >= 4)
                return RiskLevel.其他关注;
            else if (score >= 2)
                return RiskLevel.低风险;
            else
                return RiskLevel.一般;
        }

        private void WetherOccurs(WarningTaskDataModel task, WarningRecordDataModel record)
        {
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            if (task == null)
                return;

            var multiWarning = task.WarningIndex.FirstOrDefault(x => x.WarningType == WarningType.多发预警);
            var riskWarning = task.WarningIndex.FirstOrDefault(x => x.WarningType == WarningType.风险预警);
            var againWarning = task.WarningIndex.FirstOrDefault(x => x.WarningType == WarningType.再发预警);

            if (multiWarning != null)
            {
                switch (multiWarning.IndexName)
                {
                    case WarningIndexNames.近三个月相对上升率:
                        _ = float.TryParse(multiWarning.Value, out float _indexValue);

                        foreach (var item in record.MultipleMetrics)
                        {
                            var lastThreeCount = item.Value.LastThreeMonthCount.Values.Sum();
                            if (lastThreeCount >= 4 && item.Value.LastThreeMonthAscentRate >= _indexValue / 100)
                            {
                                item.Value.IsWarning = true;
                            }
                        }

                        flag1 = record.MultipleMetrics.Values.Any(x => x.IsWarning);
                        break;
                    case WarningIndexNames.近三个月发生件数:
                        _ = int.TryParse(multiWarning.Value, out int _count);

                        foreach (var item in record.MultipleMetrics)
                        {
                            var lastThreeCount = item.Value.LastThreeMonthCount.Values.Sum();
                            if (lastThreeCount != 0 && lastThreeCount >= _count)
                            {
                                item.Value.IsWarning = true;
                            }
                        }

                        flag1 = record.MultipleMetrics.Values.Any(x => x.IsWarning);
                        break;
                    case WarningIndexNames.年款不良率:
                        var selectedYear = multiWarning.Value;

                        foreach (var item in record.MultipleMetrics)
                        {
                            var std_defectRate = item.Value.DefectRateByYearModel.ContainsKey(selectedYear) ? item.Value.DefectRateByYearModel[selectedYear] : 0;
                            if (item.Value.DefectRateByYearModel.All(m => m.Value != 0 && m.Value >= std_defectRate))
                            {
                                item.Value.IsWarning = true;
                            }
                        }

                        flag1 = record.MultipleMetrics.Values.Any(x => x.IsWarning);
                        break;
                    default:
                        break;
                }

            }

            if (riskWarning != null)
            {
                //预警任务中的设置的风险等级
                var selectedRiskLevels = riskWarning.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                flag2 = riskWarning.IndexName == WarningIndexNames.风险等级 && selectedRiskLevels.Contains(record.RiskMetrics.RiskLevel?.ToString());
            }

            if (againWarning != null)
            {
                switch (againWarning.IndexName)
                {
                    case WarningIndexNames.对策后生产车辆的MQI件数:
                        int.TryParse(againWarning.Value, out int _count);
                        flag3 = record.AgainMetrics.CountAfterCntr != 0 && record.AgainMetrics.CountAfterCntr >= _count;
                        break;
                    case WarningIndexNames.对策后再发不良率:
                        float.TryParse(againWarning.Value, out float defectRate);
                        flag3 = record.AgainMetrics.DefectRateAfterCntr != 0d && record.AgainMetrics.DefectRateAfterCntr >= defectRate;
                        break;
                    default:
                        break;
                }
            }

            record.IsMultipleWarning = flag1;
            record.IsRiskWarning = flag2;
            record.IsAgainWarning = flag3;
        }
    }
}

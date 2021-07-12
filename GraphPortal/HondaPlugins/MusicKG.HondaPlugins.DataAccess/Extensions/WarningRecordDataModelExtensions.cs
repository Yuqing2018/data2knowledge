using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Resources;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace MusicKG.HondaPlugins.DataAccess.Extensions
{
    public static class WarningRecordDataModelExtensions
    {
        public static IList<string> ToModelTrainingData(this WarningRecordDataModel record, List<AIRiskWarningLevelMappingDataModel> mapping)
        {
            var mqiCount = record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.MQI.ToString()) ? record.RiskMetrics.DataSourceMetrics[DataSource.MQI.ToString()].TotalCount : 0;
            var mqiDefectRate = record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.MQI.ToString()) ? record.RiskMetrics.DataSourceMetrics[DataSource.MQI.ToString()].DefectRate : 0d;
            var mqiLastThreeMonthAscentRate = record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.MQI.ToString()) ? record.RiskMetrics.DataSourceMetrics[DataSource.MQI.ToString()].LastThreeMonthAscentRate : 0d;
            var hotlineCount = record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.HOTLINE.ToString()) ? record.RiskMetrics.DataSourceMetrics[DataSource.HOTLINE.ToString()].TotalCount : 0;
            var govCount = record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.GOV.ToString()) ? record.RiskMetrics.DataSourceMetrics[DataSource.GOV.ToString()].TotalCount : 0;
            var mediaCount = record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.MEDIA_MAIN.ToString()) ? record.RiskMetrics.DataSourceMetrics[DataSource.MEDIA_MAIN.ToString()].TotalCount : 0;
            var meanCostRepair = record.RiskMetrics.MeanCostRepair;
            var syndromeBadGrade = record.SyndromeBadGrade;
            var pushStatus = record.RiskMetrics.PushStatus ?? PushStatus.未立项;
            var permanentCntrStatus = record.RiskMetrics.PermanentCntrStatus ?? PermanentCntrStatus.空白;
            var isExcessive = record.RiskMetrics.IsExcessive.HasValue && record.RiskMetrics.IsExcessive.Value;

            return new List<string>
            {
                mqiCount.ToString(),
                mqiDefectRate.ToString(),
                mqiLastThreeMonthAscentRate.ToString(),
                hotlineCount.ToString(),
                govCount.ToString(),
                mediaCount.ToString(),
                meanCostRepair.ToString(),
                "否",
                record.RiskMetrics.AIRiskLevel?.ToString(),
                syndromeBadGrade == BadGrade.A ? "1" : "0",
                syndromeBadGrade == BadGrade.B ? "1" : "0",
                syndromeBadGrade == BadGrade.C ? "1" : "0",
                pushStatus == PushStatus.完了 ? "1" : "0",
                pushStatus == PushStatus.推进中 ? "1" : "0",
                pushStatus == PushStatus.未立项 ? "1" : "0",
                pushStatus == PushStatus.监视 ? "1" : "0",
                pushStatus == PushStatus.需再立项 ? "1" : "0",
                permanentCntrStatus == PermanentCntrStatus.空白 ? "1" : "0",
                permanentCntrStatus == PermanentCntrStatus.已对策 ? "1" : "0",
                permanentCntrStatus == PermanentCntrStatus.已明确 ? "1" : "0",
                permanentCntrStatus == PermanentCntrStatus.无需对策 ? "1" : "0",
                permanentCntrStatus == PermanentCntrStatus.未明确 ? "1" : "0",
                isExcessive ? "1" : "0",
                GetOrAddRiskLevelInModel(record.RiskMetrics.AIRiskLevel ?? RiskLevel.低风险, mapping).ToString()
            };
        }

        private static uint GetOrAddRiskLevelInModel(RiskLevel riskLevel, List<AIRiskWarningLevelMappingDataModel> mapping)
        {
            var data = mapping.FirstOrDefault(m => m.RiskLevel == riskLevel);

            if (data == null)
            {
                var levelInModel = mapping.Max(m => m.RiskLevelInModel) + 1;
                mapping.Add(new AIRiskWarningLevelMappingDataModel
                {
                    Id = ObjectId.GenerateNewId(),
                    RiskLevel = riskLevel,
                    RiskLevelInModel = levelInModel
                });
                return levelInModel;
            }
            else
                return data.RiskLevelInModel;
        }

        public static Dictionary<string, object> ToModelInput(this WarningRecordDataModel record)
        {
            if (record.RiskMetrics == null)
            {
                return new Dictionary<string, object>()
                    {
                        { "MQI渠道-件数（件）", 0 },
                        { "MQI渠道-不良率（件/台）", 0d },
                        { "MQI渠道-近3个月增长率", 0d },
                        { "800-件数（件）", 0 },
                        { "总局-件数（件）", 0 },
                        { "网络媒体-件数（件）", 0 },
                        { "风险特征-保修金额", 0 },
                        { "故障等级", BadGrade.C.ToString() },
                        { "推进情况-推进分类", AIRiskLevelSource.ResourceManager.GetString(PushStatus.完了.ToString()) },
                        { "风险特征-对策状态", AIRiskLevelSource.ResourceManager.GetString(PermanentCntrStatus.空白.ToString()) },
                        { "风险特征-超标影响", "否" }
                    };
            }
            else
            {
                return new Dictionary<string, object>
                    {
                        { "MQI渠道-件数（件）", record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.MQI.ToString())? record.RiskMetrics.DataSourceMetrics[DataSource.MQI.ToString()].TotalCount:0},
                        { "MQI渠道-不良率（件/台）",record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.MQI.ToString())? record.RiskMetrics.DataSourceMetrics[DataSource.MQI.ToString()].DefectRate:0d},
                        { "MQI渠道-近3个月增长率", record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.MQI.ToString())? record.RiskMetrics.DataSourceMetrics[DataSource.MQI.ToString()].LastThreeMonthAscentRate:0d},
                        { "800-件数（件）", record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.HOTLINE.ToString())? record.RiskMetrics.DataSourceMetrics[DataSource.HOTLINE.ToString()].TotalCount:0},
                        { "总局-件数（件）", record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.GOV.ToString())? record.RiskMetrics.DataSourceMetrics[DataSource.GOV.ToString()].TotalCount:0},
                        { "网络媒体-件数（件）",record.RiskMetrics.DataSourceMetrics.ContainsKey(DataSource.MEDIA_MAIN.ToString())? record.RiskMetrics.DataSourceMetrics[DataSource.MEDIA_MAIN.ToString()].TotalCount:0},
                        { "风险特征-保修金额", record.RiskMetrics.MeanCostRepair},
                        { "故障等级", record.SyndromeBadGrade.ToString()},
                        { "推进情况-推进分类", AIRiskLevelSource.ResourceManager.GetString((record.RiskMetrics.PushStatus??PushStatus.未立项).ToString())},
                        { "风险特征-对策状态", AIRiskLevelSource.ResourceManager.GetString((record.RiskMetrics.PermanentCntrStatus??PermanentCntrStatus.空白).ToString())},
                        { "风险特征-超标影响", record.RiskMetrics.IsExcessive.HasValue && record.RiskMetrics.IsExcessive.Value ?"是":"否"},
                    };
            }
        }
    }
}

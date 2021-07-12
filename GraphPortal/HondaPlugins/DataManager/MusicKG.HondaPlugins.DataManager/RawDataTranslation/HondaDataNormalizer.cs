using MusicKG.HondaPlugins.DataManager.Helpers;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Resources;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.Scheduler.Engine.Exceptions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaDataNormalizer : DataNormalizer<AnnotationDocumentItem>
    {
        private readonly ResourceManager resource;
        private readonly VehicleDataConstructorSettings settings;
        private readonly IRawDbContext rawDbcontext;
        private readonly IHondaMongoDbContext hondaDbContext;

        public HondaDataNormalizer(IHondaMongoDbContext hondaDbContext, IRawDbContext rawDbcontext, VehicleDataConstructorSettings settings,
            ILogger<HondaDataNormalizer> logger) : base(logger)
        {
            resource = Columns.ResourceManager;
            this.settings = settings;
            this.rawDbcontext = rawDbcontext;
            this.hondaDbContext = hondaDbContext;
            ExecutorType = HondaExecutors.HondaDataNormalizer.ToString();
        }

        protected override IEnumerable<List<AnnotationDocumentItem>> NormalizeData(string actionId,
            DataTranslatorContext context,
            DataNormalizationOptions options,
            IEnumerable<List<Dictionary<string, object>>> items)
        {
            if (!Enum.TryParse<DataSource>(context.Parameters.DataSourceName, out var dataSource))
                dataSource = DataSource.MQI;

            if (!settings.VehicleDataSettings.TryGetValue(context.Parameters.DataSourceName, out var setting))
                throw new JobExecuteException($"There is no vehicle settings for data source {context.Parameters.DataSourceName}.", actionId);

            var ignoredVehicles = hondaDbContext.IgnoredVehicle.AsQueryable().FirstOrDefault()?.Data;

            return items?.Select(item =>
            {
                var result = new List<AnnotationDocumentItem>();

                foreach (var row in item)
                {
                    // 广本定制逻辑，蒋工确认。
                    if (dataSource == DataSource.HOTLINE && row.ContainsKey("CLASSIFY_NAME"))
                    {
                        var category = row["CLASSIFY_NAME"]?.ToString();

                        if (category != "产品投诉")
                            continue;
                    }

                    var vehicle = GetVehicleData(dataSource, setting, row);

                    vehicle.SyncTimestamp = context.TaskRunTime;

                    if (!IsIgnore(ignoredVehicles, vehicle))
                        result.Add(vehicle);
                }

                return result;
            });
        }

        private bool IsIgnore(Dictionary<string, List<string>> ignoreSettings, AnnotationDocumentItem vehicle)
        {
            if (ignoreSettings == null || !ignoreSettings.ContainsKey(vehicle.CarModel))
                return false;

            var ignoredCarTypes = ignoreSettings[vehicle.CarModel];

            return ignoredCarTypes?.Contains(vehicle.CarType) ?? false;
        }

        private AnnotationDocumentItem GetVehicleData(DataSource dataSource, VehicleDataSettings setting, Dictionary<string, object> data)
        {
            var features = GetItems(dataSource, data, setting.KeyFeatures);

            logger.LogInformation($"Relate data for {GetStringFeature(features, setting.RawIdFeature, Guid.NewGuid().ToString())}");

            var vin = GetStringFeature(features, setting.VINFeature, ConstantSettings.UnknownString);

            var (carTypeInRaw, modelYearInRaw) = GetCarTypeAndModelYear(vin,
                GetStringFeature(features, setting.CarTypeFeature, ConstantSettings.UnknownString),
                GetStringFeature(features, setting.ModelYearFeature, ConstantSettings.UnknownString));

            var mileAge = GetIntFeature(features, setting.MileAgeFeature);

            var (carModel, carType, modelYear, productionDate, initDate) = GetInfoFromSalesData(vin,
                GetStringFeature(features, setting.CarModelFeature, ConstantSettings.UnknownString),
                carTypeInRaw, modelYearInRaw,
                GetDateTimeFeature(features, setting.ProductionDateFeature),
                GetDateTimeFeature(features, setting.InitialRegistDateFeature));

            var faultDate = GetDateTimeFeature(features, setting.FaultDateFeature);

            var (dealerCD, dealerName, region, province) = GetRegion(
                GetStringFeature(features, setting.DealerCDFeature, ConstantSettings.UnknownString),
                GetStringFeature(features, setting.DealerNameFeature, ConstantSettings.UnknownString),
                GetStringFeature(features, setting.RegionFeature, ConstantSettings.UnknownString),
                GetStringFeature(features, setting.ProvinceFeature, ConstantSettings.UnknownString));

            var partNo = dataSource == DataSource.MQI ? GetPartNo(carModel, features, setting.PartNoFeature, ConstantSettings.UnknownString) : ConstantSettings.UnknownString;

            var partName = dataSource == DataSource.MQI ? GetPartName(carModel, partNo, GetStringFeature(features, setting.PartNameFeature, ConstantSettings.UnknownString)) : ConstantSettings.UnknownString;

            return new AnnotationDocumentItem
            {
                Id = Guid.NewGuid().ToString(),
                RawId = GetStringFeature(features, setting.RawIdFeature, Guid.NewGuid().ToString()),
                CarModel = carModel,
                CarType = carType,
                ModelYear = modelYear,
                FrameNo = vin,
                MileAge = mileAge,
                Region = region,
                Province = province,
                DealerCD = dealerCD,
                DealerName = dealerName,
                ProductionDate = productionDate,
                InitialRegistDate = initDate,
                FaultDate = faultDate,
                PartNo = partNo,
                CostRepair = GetFloatFeature(features, setting.CostRepairFeature),
                OriginalResult = new MarkResult
                {
                    IsAddForTraining = false,
                    PartName = new PredictResult { Value = partName },
                    Syndrome = new PredictResult { Value = GetStringFeature(features, setting.SyndromeFeature, "") }
                },
                Features = features
            };
        }

        private Dictionary<string, ItemFeature> GetItems(DataSource dataSource, Dictionary<string, object> data, List<string> keyFeatures)
        {
            return data?.ToDictionary(k => resource.GetString($"{dataSource}_{k.Key}") ?? k.Key, v =>
            {
                var resourceKey = $"{dataSource}_{v.Key}";
                var translatedKey = resource.GetString(resourceKey);
                return new ItemFeature()
                {
                    Value = v.Value.ToString(),
                    KeyFeature = keyFeatures.Contains(string.IsNullOrWhiteSpace(translatedKey) ? v.Key : translatedKey)
                };
            });
        }

        private (string, string) GetCarTypeAndModelYear(string vin, string carTypeInRawData, string modelYearInRawData)
        {
            var (carType, modelYear) = VINHelper.ParseVIN(vin);

            return (carType ?? carTypeInRawData, modelYear ?? modelYearInRawData);
        }

        private string GetPartName(string carModel, string partNo, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(partNo) || partNo == ConstantSettings.UnknownString)
                return defaultValue;

            var part = hondaDbContext.Parts.AsQueryable().FirstOrDefault(p => p.No == partNo);

            // 广本写死定制逻辑，蒋工确认
            if (part == null && carModel == "EA6" && defaultValue != ConstantSettings.UnknownString)
                defaultValue = $"EA6_{defaultValue}";

            return part?.Names?.FirstOrDefault() ?? defaultValue;
        }

        private (string carModel, string carType, string modelYear, DateTime productionDate, DateTime initialRegistDate) GetInfoFromSalesData(
            string vin,
            string carModelInRawData,
            string carTypeInRawData,
            string modelYearInRawData,
            DateTime productionDateInRawData,
            DateTime initDateInRawData)
        {
            if (vin == ConstantSettings.UnknownString)
                return (carModelInRawData, carTypeInRawData, modelYearInRawData, productionDateInRawData, initDateInRawData);

            var result = rawDbcontext.RawSalesData.AsQueryable()
                .Select(s => new { s.CarModel, s.CarType, s.ModelYear, s.FrameNo, s.ProductionDate, s.InitialRegistDate })
                .FirstOrDefault(s => s.FrameNo == vin);

            var carModel = string.IsNullOrWhiteSpace(result?.CarModel) ? carModelInRawData : result.CarModel.Trim();
            var carType = string.IsNullOrWhiteSpace(result?.CarType) ? carTypeInRawData : result.CarType.Trim();
            var modelYear = string.IsNullOrWhiteSpace(result?.ModelYear) ? modelYearInRawData : result.ModelYear.Trim();
            var productionDate = ParseDateTime(result?.ProductionDate?.Trim(), productionDateInRawData);
            var initialRegistDate = ParseDateTime(result?.InitialRegistDate?.Trim(), initDateInRawData);

            return (carModel, carType, modelYear, productionDate, initialRegistDate);
        }

        private (string dealerCD, string dealerName, string region, string province) GetRegion(string dealerCDInRawData, string dealerNameInRawData, string regionInRawData, string provinceInRawData)
        {
            if ((dealerCDInRawData == ConstantSettings.UnknownString && dealerNameInRawData == ConstantSettings.UnknownString) ||
                (dealerCDInRawData != ConstantSettings.UnknownString && dealerNameInRawData != ConstantSettings.UnknownString && regionInRawData != ConstantSettings.UnknownString && provinceInRawData != ConstantSettings.UnknownString))
                return (dealerCDInRawData, dealerNameInRawData, regionInRawData, provinceInRawData);

            var querable = rawDbcontext.RawRegionData.AsQueryable()
                .Select(m => new { m.DealerCD, m.DealerName, m.Region, m.Province });

            if (dealerCDInRawData == ConstantSettings.UnknownString)
                querable = querable.Where(m => m.DealerName == dealerNameInRawData);
            else
                querable = querable.Where(m => m.DealerCD == dealerCDInRawData);

            var result = querable.FirstOrDefault();

            var dealerCD = string.IsNullOrWhiteSpace(result?.DealerCD) ? dealerCDInRawData : result.DealerCD.Trim();
            var dealerName = string.IsNullOrWhiteSpace(result?.DealerName) ? dealerNameInRawData : result.DealerName.Trim();
            var region = string.IsNullOrWhiteSpace(result?.Region) ? regionInRawData : result.Region.Trim();
            var province = string.IsNullOrWhiteSpace(result?.Province) ? provinceInRawData : result.Province.Trim();

            return (dealerCD, dealerName, region, province);
        }

        private string GetPartNo(string carModel, Dictionary<string, ItemFeature> features, string key, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key) || !features.ContainsKey(key) ||
                string.IsNullOrWhiteSpace(features[key].Value?.ToString()) ||
                features[key].Value?.ToString().Length < 5)
                return defaultValue;

            var partNo = features[key].Value.ToString().Substring(0, 5);

            // 广本写死定制逻辑，蒋工确认
            if (carModel == "EA6")
                partNo = $"EA6_{partNo}";

            return partNo;
        }

        private string GetStringFeature(Dictionary<string, ItemFeature> features, string key, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key) || !features.ContainsKey(key) || string.IsNullOrWhiteSpace(features[key].Value?.ToString()))
                return defaultValue;

            return features[key].Value?.ToString();
        }

        private int GetIntFeature(Dictionary<string, ItemFeature> features, string key)
        {
            if (string.IsNullOrWhiteSpace(key) || !features.ContainsKey(key) || !int.TryParse(features[key]?.Value?.ToString(), out var result))
                return -1;

            return result;
        }

        private float GetFloatFeature(Dictionary<string, ItemFeature> features, string key, float defaultValue = 0f)
        {
            if (string.IsNullOrWhiteSpace(key) || !features.ContainsKey(key) || !float.TryParse(features[key]?.Value?.ToString(), out var result))
                return defaultValue;

            return result;
        }

        private DateTime GetDateTimeFeature(Dictionary<string, ItemFeature> features, string key)
        {
            if (string.IsNullOrWhiteSpace(key) || !features.ContainsKey(key))
                return DateTime.MaxValue;

            return ParseDateTime(features[key]?.Value?.ToString(), DateTime.MaxValue);
        }

        private DateTime ParseDateTime(string dateTime, DateTime defaultValue)
        {
            DateTime result = defaultValue;

            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                if (DateTime.TryParse(dateTime, out result))
                    return result;

                if (DateTime.TryParseExact(dateTime, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out result))
                    return result;
            }

            return result;
        }
    }
}

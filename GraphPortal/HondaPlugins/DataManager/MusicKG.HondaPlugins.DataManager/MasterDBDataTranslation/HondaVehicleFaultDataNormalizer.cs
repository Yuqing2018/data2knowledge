using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaVehicleFaultDataNormalizer : DataNormalizer<AnnotationDocumentItem>
    {
        private readonly VehicleDataConstructorSettings vehicleDataConstructorSettings;

        public HondaVehicleFaultDataNormalizer(VehicleDataConstructorSettings vehicleDataConstructorSettings, ILogger<HondaVehicleFaultDataNormalizer> logger) 
            : base(logger)
        {
            this.vehicleDataConstructorSettings = vehicleDataConstructorSettings;
            ExecutorType = HondaExecutors.HondaVehicleFaultDataNormalizer.ToString();
        }

        protected override IEnumerable<List<AnnotationDocumentItem>> NormalizeData(string actionId, 
            DataTranslatorContext context, 
            DataNormalizationOptions options, 
            IEnumerable<List<Dictionary<string, object>>> items)
        {
            return items?.Select(data =>
            {
                return data.Select(item =>
                {
                    var dataSource = item[nameof(VehicleFaultDataModel.DataSource)]?.ToString();

                    var setting = vehicleDataConstructorSettings.VehicleDataSettings[dataSource];

                    var rawId = item[nameof(VehicleFaultDataModel.RawId)].ToString();

                    return new AnnotationDocumentItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        RawId = rawId,
                        DataSourceName = item[nameof(VehicleFaultDataModel.DataSource)]?.ToString(),
                        PartNo = GetStringFeature(item, setting.PartNoFeature, ConstantSettings.UnknownString),
                        OriginalResult = new MarkResult
                        {
                            IsAddForTraining = false,
                            PartName = new PredictResult
                            {
                                Value = GetStringFeature(item, setting.PartNameFeature, ConstantSettings.UnknownString)
                            }
                        },
                        Features = item?.ToDictionary(k => k.Key, v => new ItemFeature
                        {
                            Value = v.Value.ToString(),
                            KeyFeature = setting.KeyFeatures.Contains(v.Key)
                        })
                    };
                }).ToList();
            }); 
        }

        private string GetStringFeature(Dictionary<string, object> features, string key, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key) || !features.ContainsKey(key) || string.IsNullOrWhiteSpace(features[key]?.ToString()))
                return defaultValue;

            return features[key]?.ToString();
        }
    }
}

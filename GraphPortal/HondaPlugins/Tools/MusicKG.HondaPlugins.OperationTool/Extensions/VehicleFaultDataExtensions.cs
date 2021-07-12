using MusicKG.DataManager.Models;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicKG.HondaPlugins.OperationTool.Extensions
{
    public static class VehicleFaultDataExtensions
    {
        public static ModelTrainingData ToModelTrainingData(this VehicleFaultDataModel vehicleFaultData, VehicleDataConstructorSettings settings)
        {
            var setting = settings.VehicleDataSettings[vehicleFaultData.DataSource.ToString()];

            var result = new ModelTrainingData
            {
                Id = string.IsNullOrWhiteSpace(vehicleFaultData.RawId) ? Guid.NewGuid().ToString() : vehicleFaultData.RawId,
                DataSource = vehicleFaultData.DataSource.ToString(),
                Timestamp = DateTime.UtcNow,
                ModelName = HondaModelNames.TextClassificationModel.ToString(),
                InputFeatures = setting.KeyFeatures.ToDictionary(k => k, v => vehicleFaultData.Features[v]),
                OutputFeatures = new Dictionary<string, string>
                {
                    { nameof(vehicleFaultData.Syndrome), vehicleFaultData.Syndrome },
                    { nameof(vehicleFaultData.PartName), vehicleFaultData.PartName },
                }
            };

            return result;
        }
    }
}

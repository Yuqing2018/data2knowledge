using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using System.Linq;
using System;

namespace MusicKG.HondaPlugins.DataAccess.Extensions
{
    public static class AnnotationDocumentExtensions
    {
        public static VehicleFaultDataModel ToVehicleFaultDataModel(this AnnotationDocumentItem item, DataSource dataSouce, DateTime runTime)
        {
            VehicleFaultDataModel result = new VehicleFaultDataModel();

            result.DataSource = dataSouce;
            result.RawId = item.RawId;
            result.CarModel = item.CarModel;
            result.CarType = item.CarType;
            result.ModelYear = item.ModelYear;
            result.DealerCD = item.DealerCD;
            result.DealerName = item.DealerName;
            result.DealerName = item.DealerName;
            result.FaultDate = item.FaultDate;
            result.FrameNo = item.FrameNo;
            result.InitialRegistDate = item.InitialRegistDate;
            result.MileAge = item.MileAge;
            result.Region = item.Region;
            result.Province = item.Province;
            result.ProductionDate = item.ProductionDate;
            result.PartNo = item.PartNo;
            result.CostRepair = item.CostRepair;
            result.PartName = GetPartName(item);
            result.Syndrome = GetSyndrome(item);
            result.RelatedInfo = item.RelatedInfo;
            result.Timestamp = runTime;
            result.SyncTimestamp = item.SyncTimestamp;
            result.Features = item.Features.ToDictionary(k => k.Key, v => v.Value.Value);

            return result;
        }

        private static string GetPartName(AnnotationDocumentItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.Result?.PartName?.Value) &&
                item.Result.PartName.Value != ConstantSettings.UnknownString)
                return item.Result.PartName.Value;

            if (!string.IsNullOrWhiteSpace(item.ModelResult?.PartName?.Value) &&
                item.ModelResult.PartName.Value != ConstantSettings.UnknownString)
                return item.ModelResult.PartName.Value;

            if (!string.IsNullOrWhiteSpace(item.OriginalResult?.PartName?.Value) &&
                item.OriginalResult.PartName.Value != ConstantSettings.UnknownString)
                return item.OriginalResult.PartName.Value;

            return ConstantSettings.UnknownString;
        }

        private static string GetSyndrome(AnnotationDocumentItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.Result?.Syndrome?.Value) &&
                item.Result.Syndrome.Value != ConstantSettings.UnknownString)
                return item.Result.Syndrome.Value;

            if (!string.IsNullOrWhiteSpace(item.ModelResult?.Syndrome?.Value) &&
                item.ModelResult.Syndrome.Value != ConstantSettings.UnknownString)
                return item.ModelResult.Syndrome.Value;

            return ConstantSettings.UnknownString;
        }
    }
}

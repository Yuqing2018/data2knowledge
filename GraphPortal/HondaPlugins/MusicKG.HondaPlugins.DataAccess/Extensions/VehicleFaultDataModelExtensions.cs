using MusicKG.DataManager.Models.Settings;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using System;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.DataAccess.Extensions
{
    public static class VehicleFaultDataModelExtensions
    {
        public static AnnotationDocumentItem ToAnnotationItem(this VehicleFaultDataModel data, LabelingServiceSettings labelingServiceSettings)
        {
            return new AnnotationDocumentItem
            {
                CarModel = data.CarModel,
                CarType = data.CarType,
                FaultDate = data.FaultDate,
                MileAge = data.MileAge,
                CostRepair = data.CostRepair,
                DataSourceName = data.DataSource.ToString(),
                DealerCD = data.DealerCD,
                DealerName = data.DealerName,
                FrameNo = data.FrameNo,
                Id = Guid.NewGuid().ToString(),
                InitialRegistDate = data.InitialRegistDate,
                ModelYear = data.ModelYear,
                PartNo = data.PartNo,
                ProductionDate = data.ProductionDate,
                RawId = data.RawId,
                Region = data.Region,
                Province = data.Province,
                RelatedInfo = data.RelatedInfo,
                Features = GetFeatures(data.Features, labelingServiceSettings)
            };
        }

        private static Dictionary<string, ItemFeature> GetFeatures(Dictionary<string, string> features, LabelingServiceSettings labelingServiceSettings)
        {
            throw new NotImplementedException();
        }
    }
}

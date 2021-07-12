using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.PredictWrapper.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.PredictWrapper
{
    public interface IVehiclePredictor
    {
        Task<List<PredictResultServiceModel>> PredictBatchAsync(
            List<AnnotationDocumentItem> items,
            bool predictPart = true,
            bool predictSyndrome = true);

        PredictResultServiceModel Predict(
            AnnotationDocumentItem items,
            bool predictPart = true,
            bool predictSyndrome = true);

        void Initialize();

        void Close();
    }
}

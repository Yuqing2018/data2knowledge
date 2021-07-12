using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;

namespace MusicKG.HondaPlugins.PredictWrapper.ServiceModels
{
    public class PredictResultServiceModel
    {
        public string ItemId { get; set; }

        public PredictResult PartName { get; set; }

        public PredictResult Syndrome { get; set; }
    }
}

using MusicKG.DataManager.ModelTrainer.Settings;

namespace MusicKG.HondaPlugins.OperationTool.Settings
{
    public class HondaRiskModelTrainingSettings : ModelTrainingSettings
    {
        public string TrainingFileName { get; set; } = "RiskModelTrainingData.xlsx";

        public string InitialTrainingFullFileName { get; set; } = "InitialTrainingData.xlsx";

        public string ModelFileName { get; set; } = "risk_prediction_model.pkl";

        public string RiskModelInitUrl { get; set; } = "http://localhost:40076/api/init";
    }
}

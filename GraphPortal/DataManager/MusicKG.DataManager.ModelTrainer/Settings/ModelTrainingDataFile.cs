namespace MusicKG.DataManager.ModelTrainer.Settings
{
    public class ModelTrainingDataFile
    {
        public string TrainDataFileName { get; set; } = "train";

        public string ValidationDataFile { get; set; } = "val";

        public string TestDataFile { get; set; } = "test";
    }
}
namespace MusicKG.DataManager.ModelTrainer.Settings
{
    public class ModelTrainingSettings
    {
        public string ModelName { get; set; }

        public string ModelTrainingScriptFile { get; set; }

        public string ModelServingScriptFile { get; set; }

        public string RevertModelServingScriptFile { get; set; }

        public string ModelServingScriptFolder { get; set; }

        public string ModelTrainingScriptFolder { get; set; }

        public string TrainingDataFolder { get; set; }

        public ModelTrainingDataFile TrainingFiles { get; set; }

        public string TrainedModelFolder { get; set; }

        public int TrainingTimeoutInHours { get; set; }

        public string ModelServingLocation { get; set; }

        public string ModelBackupLocation { get; set; }
    }
}
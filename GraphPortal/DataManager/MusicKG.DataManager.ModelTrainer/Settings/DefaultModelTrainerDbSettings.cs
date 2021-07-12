namespace MusicKG.DataManager.ModelTrainer.Settings
{
    public class DefaultModelTrainerDbSettings
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public string TrainingDataTableName { get; set; }

        public string TrainingDataTimestampFieldName { get; set; } = "Timestamp";

        public string TrainingHistoryTableName { get; set; }
    }
}

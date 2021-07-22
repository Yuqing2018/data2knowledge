using MusicKG.DataManager.ModelTrainer.Settings;

namespace MusicKG.HondaPlugins.ModelTrainer.Settings
{
    public class HondaClassificationModelTrainingSettings : ModelTrainingSettings
    {
        public int TrainingDataCount { get; set; } = 800000;

        public PartNameModelTrainingDataFiles PartNameFiles { get; set; }

        public SyndromeModelTrainingDataFiles SyndromeFiles { get; set; }
    }

    public class PartNameModelTrainingDataFiles : ModelTrainingDataFile
    {
        public string PartTrainingDataFolder { get; set; } = "part";

        public string PartNamesFileName { get; set; } = "new_partsname.txt";
    }

    public class SyndromeModelTrainingDataFiles : ModelTrainingDataFile
    {
        public string SyndromeTrainingDataFolder { get; set; } = "syndrome";

        public string SyndromesFileName { get; set; } = "new_syndromelist.txt";

        public string SyndromeIdMappingFileName { get; set; } = "AllSyndrome.json";
    }
}
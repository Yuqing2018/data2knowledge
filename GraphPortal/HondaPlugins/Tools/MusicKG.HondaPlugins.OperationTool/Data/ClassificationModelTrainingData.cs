using MusicKG.DataManager.Models;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.OperationTool.Data
{
    public class HondaModelTrainingData
    {
        public ClassificationModelTrainingData PartModelTrainingData { get; set; }

        public ClassificationModelTrainingData SyndromeModelTrainingData { get; set; }
    }

    public class ClassificationModelTrainingData
    {
        public IEnumerable<ModelTrainingData> TrainData { get; set; }

        public IEnumerable<ModelTrainingData> ValidationData { get; set; }

        public IEnumerable<ModelTrainingData> TestData { get; set; }
    }
}
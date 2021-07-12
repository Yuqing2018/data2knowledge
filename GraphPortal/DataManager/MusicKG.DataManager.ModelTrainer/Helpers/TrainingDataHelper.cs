using System;

namespace MusicKG.DataManager.ModelTrainer.Helpers
{
    public static class TrainingDataHelper
    {
        private const long BigDataThreshold = 1000000;

        public static (int trainCount, int validationCount, int testCount) AssignTrainingData(int totalDataCount, bool ignoreTest)
        {
            var isBigData = totalDataCount > BigDataThreshold;

            var trainingCount = (int)Math.Ceiling(totalDataCount * (isBigData ? 0.98 : 0.8));

            var validationCount = ignoreTest ? totalDataCount - trainingCount :
                    (int)Math.Ceiling(totalDataCount * (isBigData ? 0.01 : 0.1));

            var testCount = totalDataCount - trainingCount - validationCount;

            return (trainingCount, validationCount, testCount);
        }
    }
}

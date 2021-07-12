using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.HondaPlugins.OperationTool.Settings;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Utility;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using System;

namespace MusicKG.HondaPlugins.OperationTool.Executors
{
    public class ClassificationModelTrainer : DataManager.ModelTrainer.Executors.Abstractions.ModelTrainer
    {
        private new readonly HondaClassificationModelTrainingSettings settings;

        public ClassificationModelTrainer(HondaClassificationModelTrainingSettings settings, ILogger<ClassificationModelTrainer> logger) 
            : base(settings, logger)
        {
            this.settings = settings;
            ExecutorType = HondaModelTrainerExecutors.ClassificationModelTrainer.ToString();
        }

        protected async override Task<bool> TrainModelAsync(string actionId, ModelTrainerContext context, ModelTrainingOptions options, string inputFolder, string outputFolder)
        {
            if (Directory.Exists(outputFolder))
                Directory.Delete(outputFolder, true);

            return ShellHelper.RunShell(settings.ModelTrainingScriptFile,
                settings.ModelTrainingScriptFolder, TimeSpan.FromHours(settings.TrainingTimeoutInHours).TotalMilliseconds,
                error => logger.LogActionError(actionId, error),
                info => logger.LogActionInfo(actionId, info), 
                Path.Combine(inputFolder, settings.PartNameFiles.PartTrainingDataFolder, settings.PartNameFiles.TrainDataFileName),
                Path.Combine(inputFolder, settings.PartNameFiles.PartTrainingDataFolder, settings.PartNameFiles.ValidationDataFile),
                Path.Combine(inputFolder, settings.PartNameFiles.PartTrainingDataFolder, settings.PartNameFiles.TestDataFile),
                Path.Combine(inputFolder, settings.PartNameFiles.PartTrainingDataFolder, settings.PartNameFiles.PartNamesFileName),
                Path.Combine(inputFolder, settings.SyndromeFiles.SyndromeTrainingDataFolder, settings.SyndromeFiles.TrainDataFileName),
                Path.Combine(inputFolder, settings.SyndromeFiles.SyndromeTrainingDataFolder, settings.SyndromeFiles.ValidationDataFile),
                Path.Combine(inputFolder, settings.SyndromeFiles.SyndromeTrainingDataFolder, settings.SyndromeFiles.TestDataFile),
                Path.Combine(inputFolder, settings.SyndromeFiles.SyndromeTrainingDataFolder, settings.SyndromeFiles.SyndromesFileName),
                Path.Combine(inputFolder, settings.SyndromeFiles.SyndromeTrainingDataFolder, settings.SyndromeFiles.SyndromeIdMappingFileName),
                outputFolder);
        }
    }
}
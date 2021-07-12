using MusicKG.DataManager.ModelTrainer.Settings;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.Models.Enums;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Utility;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System;

namespace MusicKG.DataManager.ModelTrainer.Executors
{
    public class DefaultModelTrainer : Abstractions.ModelTrainer
    {
        public DefaultModelTrainer(ModelTrainingSettings settings, ILogger<DefaultModelTrainer> logger) 
            : base(settings, logger)
        {
            ExecutorType = ModelTrainingExecutors.DefaultModelTrainer.ToString();
        }

        protected async override Task<bool> TrainModelAsync(string actionId, 
            ModelTrainerContext context, 
            ModelTrainingOptions options,
            string inputFolder, string outputFolder)
        {
            if (Directory.Exists(outputFolder))
                Directory.Delete(outputFolder, true);

            return ShellHelper.RunShell(settings.ModelTrainingScriptFile,
                settings.ModelTrainingScriptFolder, TimeSpan.FromHours(settings.TrainingTimeoutInHours).TotalMilliseconds,
                error => logger.LogActionError(actionId, error),
                info => logger.LogActionInfo(actionId, info), inputFolder, outputFolder);
        }
    }
}

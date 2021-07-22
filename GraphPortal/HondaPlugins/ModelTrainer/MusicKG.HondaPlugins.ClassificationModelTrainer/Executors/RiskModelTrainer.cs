using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.HondaPlugins.ModelTrainer.Settings;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.ModelTrainer.Executors
{
    public class RiskModelTrainer : DataManager.ModelTrainer.Executors.Abstractions.ModelTrainer
    {
        private new readonly HondaRiskModelTrainingSettings settings;

        public RiskModelTrainer(HondaRiskModelTrainingSettings settings,
            ILogger<RiskModelTrainer> logger) : base(settings, logger)
        {
            this.settings = settings;
        }

        protected async override Task<bool> TrainModelAsync(string actionId, 
            ModelTrainerContext context, 
            ModelTrainingOptions options, 
            string inputFolder, string outputFolder)
        {
            var outputFile = Path.Combine(outputFolder, settings.ModelFileName);

            return ShellHelper.RunShell(settings.ModelTrainingScriptFile,
                settings.ModelTrainingScriptFolder, TimeSpan.FromHours(settings.TrainingTimeoutInHours).TotalMilliseconds,
                error => logger.LogActionError(actionId, error),
                info => logger.LogActionInfo(actionId, info),
                Path.Combine(inputFolder, settings.TrainingFileName),
                outputFile);
        }
    }
}

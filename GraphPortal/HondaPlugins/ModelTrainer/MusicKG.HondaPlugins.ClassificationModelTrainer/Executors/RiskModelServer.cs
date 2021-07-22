using MusicKG.DataManager.ModelTrainer.Executors;
using MusicKG.HondaPlugins.ModelTrainer.Settings;
using Microsoft.Extensions.Logging;
using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;
using System.Threading.Tasks;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.Utility;
using System;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Engine.Extensions;

namespace MusicKG.HondaPlugins.ModelTrainer.Executors
{
    public class RiskModelServer : ModelServer
    {
        private new HondaRiskModelTrainingSettings settings;

        public RiskModelServer(HondaRiskModelTrainingSettings settings, ILogger<RiskModelServer> logger)
            : base(settings, logger)
        {
            this.settings = settings;
            ExecutorType = HondaModelTrainerExecutors.RiskModelServer.ToString();
        }

        protected override async Task<int> UpgradeModelAsync(string actionId,
            ModelTrainerContext context,
            ModelServingOptions options,
            string trainedModelFolder)
        {
            var version = context.LastModelVersion + 1;

            bool result = ShellHelper.RunShell(settings.ModelServingScriptFile,
                settings.ModelServingScriptFolder, TimeSpan.FromHours(1).TotalMilliseconds,
                error => logger.LogActionError(actionId, error),
                info => logger.LogActionInfo(actionId, info),
                settings.ModelServingLocation,
                settings.ModelBackupLocation,
                context.LastModelVersion.ToString(),
                version.ToString(),
                trainedModelFolder,
                settings.RiskModelInitUrl);

            if (!result)
                throw new JobExecuteException($"Serving model from {trainedModelFolder} failed. Find more details from app log.", actionId);

            return version;
        }

        protected async override Task RevertModelVersionAsync(string actionId, int version)
        {
            ShellHelper.RunShell(settings.RevertModelServingScriptFile,
                settings.ModelServingScriptFolder, TimeSpan.FromHours(1).TotalMilliseconds,
                error => logger.LogActionError(actionId, error),
                info => logger.LogActionInfo(actionId, info), 
                settings.ModelServingLocation,
                settings.ModelBackupLocation,
                version.ToString(),
                settings.RiskModelInitUrl);
        }
    }
}

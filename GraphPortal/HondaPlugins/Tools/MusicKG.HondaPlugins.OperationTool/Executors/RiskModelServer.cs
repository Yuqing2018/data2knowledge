using MusicKG.HondaPlugins.OperationTool.Settings;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.Utility;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace MusicKG.HondaPlugins.OperationTool.Executors
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
                settings.ModelServingScriptFolder, TimeSpan.FromHours(settings.TrainingTimeoutInHours).TotalMilliseconds,
                error => logger.LogActionError(actionId, error),
                info => logger.LogActionInfo(actionId, info),
                settings.ModelServingLocation,
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
                settings.TrainedModelFolder,
                version.ToString(),
                settings.RiskModelInitUrl);
        }
    }
}

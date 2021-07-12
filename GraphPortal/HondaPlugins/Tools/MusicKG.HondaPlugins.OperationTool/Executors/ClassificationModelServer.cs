using MusicKG.DataManager.ModelTrainer.Executors;
using MusicKG.HondaPlugins.OperationTool.Settings;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MusicKG.Utility;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.Scheduler.Engine.Extensions;
using System;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;

namespace MusicKG.HondaPlugins.OperationTool.Executors
{
    public class ClassificationModelServer : ModelServer
    {
        private new readonly HondaClassificationModelTrainingSettings settings;

        public ClassificationModelServer(HondaClassificationModelTrainingSettings settings, ILogger<ClassificationModelServer> logger)
            : base(settings, logger)
        {
            this.settings = settings;
            ExecutorType = HondaModelTrainerExecutors.ClassificationModelServer.ToString();
        }

        protected override async Task<int> UpgradeModelAsync(string actionId,
            ModelTrainerContext context,
            ModelServingOptions options,
            string trainedModelFolder)
        {
            var version = context.LastModelVersion + 1;

            bool hasError = false;

            bool result = ShellHelper.RunShell(settings.ModelServingScriptFile,
                settings.ModelServingScriptFolder, TimeSpan.FromHours(1).TotalMilliseconds,
                error => { logger.LogActionError(actionId, error); hasError = true; },
                info => logger.LogActionInfo(actionId, info),
                settings.ModelServingLocation,
                trainedModelFolder,
                version.ToString());

            if (!result || hasError)
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
                version.ToString());
        }
    }
}

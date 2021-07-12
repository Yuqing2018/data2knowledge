using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;
using MusicKG.DataManager.Models.Enums;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Exceptions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MusicKG.Utility;
using MusicKG.DataManager.ModelTrainer.Settings;
using System;

namespace MusicKG.DataManager.ModelTrainer.Executors
{
    public class DefaultModelServer : ModelServer
    {
        public DefaultModelServer(ModelTrainingSettings settings, ILogger<DefaultModelServer> logger) : base(settings, logger)
        {
            ExecutorType = ModelTrainingExecutors.DefaultModelServer.ToString();
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
                info => logger.LogActionInfo(actionId, info), context.LastModelVersion.ToString(), version.ToString(), trainedModelFolder);

            if (!result)
                throw new JobExecuteException($"Serving model from {trainedModelFolder} failed. Find more details from app log.", actionId);

            return version;
        }

        protected async override Task RevertModelVersionAsync(string actionId, int version)
        {
            bool result = ShellHelper.RunShell(settings.RevertModelServingScriptFile,
                settings.ModelServingScriptFolder, TimeSpan.FromHours(1).TotalMilliseconds,
                error => logger.LogActionError(actionId, error),
                info => logger.LogActionInfo(actionId, info), version.ToString());
        }
    }
}

using MusicKG.DataManager.ModelTrainer.Data;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Settings;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.IO;

namespace MusicKG.DataManager.ModelTrainer.Executors.Abstractions
{
    /// <summary>
    /// Customerized executor.
    /// </summary>
    public abstract class TrainingDataCollector : ActionExecutor<ModelTrainerContext, TrainingDataCollectionOptions, DefaultActionData, ModelTrainingActionData>
    {
        protected readonly ModelTrainingSettings settings;

        public TrainingDataCollector(ModelTrainingSettings settings, ILogger<TrainingDataCollector> logger)
            : base(logger)
        {
            this.settings = settings;
        }

        protected async override Task<ModelTrainingActionData> ExecuteInternalAsync(string actionId, 
            ModelTrainerContext context, 
            TrainingDataCollectionOptions options, 
            DefaultActionData data)
        {
            var task = context.Parameters;

            if (task.TrainAt > context.TaskRunTime)
            {
                logger.LogActionInfo(actionId, $"It is not time to train the model. The training time should be {task.TrainAt}");
                context.NeedTrain = false;
                return new ModelTrainingActionData { TrainingDataFolder = null };
            }

            var latestVersion = await GetLatestVersionAsync(settings.ModelName);

            if (latestVersion == null)
                latestVersion = new ModelVersion { TrainedAt = DateTime.MinValue, Version = 0 };

            var count = await CountTrainingDataAsync(actionId, context, options);
            
            logger.LogActionInfo(actionId, $"There are {count} data prepared for model training.");

            context.LastModelVersion = latestVersion.Version;
            context.TrainingDataCount = count;
            context.TrainingDataInputFolder = await SaveTrainingDataAsync(actionId, context, options);
            context.ModelName = settings.ModelName;

            return new ModelTrainingActionData
            { 
                TrainingDataFolder = context.TrainingDataInputFolder
            };
        }

        protected async override Task RevertInternalAsync(string actionId, ModelTrainerContext context)
        {
            if (!string.IsNullOrWhiteSpace(context.TrainingDataInputFolder) && Directory.Exists(context.TrainingDataInputFolder))
                Directory.Delete(context.TrainingDataInputFolder, true);
        }

        protected abstract Task<string> SaveTrainingDataAsync(string actionId, ModelTrainerContext context, TrainingDataCollectionOptions options);

        protected abstract Task<ModelVersion> GetLatestVersionAsync(string modelName);

        protected abstract Task<int> CountTrainingDataAsync(string actionId,
            ModelTrainerContext context, TrainingDataCollectionOptions options);
    }
}

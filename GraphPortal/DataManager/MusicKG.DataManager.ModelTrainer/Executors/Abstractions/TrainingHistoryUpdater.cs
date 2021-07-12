using MusicKG.DataManager.ModelTrainer.Data;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Settings;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MusicKG.DataManager.ModelTrainer.Executors.Abstractions
{
    /// <summary>
    /// Customerized executor.
    /// </summary>
    public abstract class TrainingHistoryUpdater : ActionExecutor<ModelTrainerContext, TrainingStatusUpdatingOptions, TrainingStatusUpdatingActionData, object>
    {
        public TrainingHistoryUpdater(ILogger<TrainingHistoryUpdater> logger)
            : base(logger)
        {
        }

        protected async override Task<object> ExecuteInternalAsync(string actionId, 
            ModelTrainerContext context, 
            TrainingStatusUpdatingOptions options, 
            TrainingStatusUpdatingActionData data)
        {
            if (context.TrainingDataCount == 0)
            {
                logger.LogActionInfo(actionId, "There is no data to train the model.");
                return new object();
            }

            await UpdateTrainingStatusAsync(actionId, context, options,
                data.CurrentModelVersion);

            context.CurrentModelVersion = data.CurrentModelVersion;

            return new object();
        }

        protected async override Task RevertInternalAsync(string actionId, ModelTrainerContext context)
        {
            if (context.CurrentModelVersion.HasValue)
                await RevertTrainingStatusAsync(actionId, context.ModelName, context.CurrentModelVersion.Value);
        }

        protected abstract Task RevertTrainingStatusAsync(string actionId, string modelName, int version);

        protected abstract Task UpdateTrainingStatusAsync(string actionId,
            ModelTrainerContext context, TrainingStatusUpdatingOptions options,
            int version);
    }
}

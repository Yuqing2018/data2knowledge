using MusicKG.DataManager.ModelTrainer.Data;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using MusicKG.DataManager.ModelTrainer.Settings;

namespace MusicKG.DataManager.ModelTrainer.Executors.Abstractions
{
    /// <summary>
    /// Customerized executor.
    /// </summary>
    public abstract class ModelServer : ActionExecutor<ModelTrainerContext, ModelServingOptions, ModelServingActionData, TrainingStatusUpdatingActionData>
    {
        protected readonly ModelTrainingSettings settings;

        public ModelServer(ModelTrainingSettings settings, ILogger<ModelServer> logger)
            : base(logger)
        {
            this.settings = settings;
        }

        protected async override Task<TrainingStatusUpdatingActionData> ExecuteInternalAsync(string actionId, 
            ModelTrainerContext context, 
            ModelServingOptions options, 
            ModelServingActionData data)
        {
            if (context.TrainingDataCount == 0)
            {
                logger.LogActionInfo(actionId, "There is no data to train the model.");
                return new TrainingStatusUpdatingActionData { CurrentModelVersion = -1 };
            }

            if (!Directory.Exists(data.TrainedModelFolder))
                throw new JobExecuteException("The trained model does not exist.", actionId);

            logger.LogActionInfo(actionId, $"Upgrading model version using folder {data.TrainedModelFolder}.");

            var version = await UpgradeModelAsync(actionId, context, options, data.TrainedModelFolder);

            logger.LogActionInfo(actionId, $"Model has been upgraded to version {version}.");

            return new TrainingStatusUpdatingActionData { CurrentModelVersion = version };
        }

        protected async override Task RevertInternalAsync(string actionId, ModelTrainerContext context)
        {
            logger.LogActionInfo(actionId, $"Reverting model to version {context.LastModelVersion}.");

            await RevertModelVersionAsync(actionId, context.LastModelVersion);

            logger.LogActionInfo(actionId, $"Model has been reverted to version {context.LastModelVersion}.");
        }

        protected abstract Task RevertModelVersionAsync(string actionId, int version);

        protected abstract Task<int> UpgradeModelAsync(string actionId, ModelTrainerContext context, ModelServingOptions options, string trainedModelFolder);
    }
}

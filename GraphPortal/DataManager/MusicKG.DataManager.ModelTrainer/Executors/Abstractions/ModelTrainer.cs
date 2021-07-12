using MusicKG.DataManager.ModelTrainer.Data;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Settings;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Exceptions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.IO;

namespace MusicKG.DataManager.ModelTrainer.Executors.Abstractions
{
    /// <summary>
    /// Customerized executor.
    /// </summary>
    public abstract class ModelTrainer : ActionExecutor<ModelTrainerContext, ModelTrainingOptions, ModelTrainingActionData, ModelServingActionData>
    {
        protected readonly ModelTrainingSettings settings;

        public ModelTrainer(ModelTrainingSettings settings, ILogger<ModelTrainer> logger)
            : base(logger)
        {
            this.settings = settings;
        }

        protected async override Task<ModelServingActionData> ExecuteInternalAsync(string actionId, 
            ModelTrainerContext context, 
            ModelTrainingOptions options, 
            ModelTrainingActionData data)
        {
            if (context.TrainingDataCount == 0)
            {
                logger.LogActionInfo(actionId, "There is no data to train the model.");
                return new ModelServingActionData { TrainedModelFolder = null };
            }

            if (!Directory.Exists(data.TrainingDataFolder))
                throw new JobExecuteException("The training data folder does not exist.", actionId);

            DateTime start = DateTime.UtcNow;

            var folder = CreateModelOutputFolder(context);

            logger.LogActionInfo(actionId, $"Training model using data from folder: {data.TrainingDataFolder}.");

            var result = await TrainModelAsync(actionId, context, options, data.TrainingDataFolder, folder);

            if (!result)
                throw new JobExecuteException("Train model failed, please find details from app logs.", actionId);

            logger.LogActionInfo(actionId, 
                $"Model trained succeed, duration (in hours): {(DateTime.UtcNow - start).TotalHours}, output folder: {folder}.");

            return new ModelServingActionData { TrainedModelFolder = folder };
        }

        protected async override Task RevertInternalAsync(string actionId, ModelTrainerContext context)
        {
            //Do nothing.
        }

        protected virtual string CreateModelOutputFolder(ModelTrainerContext context)
        {
            var folder = Path.Combine(settings.TrainedModelFolder, (context.LastModelVersion + 1).ToString());

            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            Directory.CreateDirectory(folder);

            return folder;
        }

        protected abstract Task<bool> TrainModelAsync(string actionId, ModelTrainerContext context, ModelTrainingOptions options, string inputFolder, string outputFolder);
    }
}

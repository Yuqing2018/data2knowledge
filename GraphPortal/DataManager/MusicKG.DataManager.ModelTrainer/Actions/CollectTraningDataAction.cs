using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.Models.Enums;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.DataManager.ModelTrainer.Actions
{
    /// <summary>
    /// Customerized job action.
    /// </summary>
    public class CollectTraningDataAction : JobAction<ModelTrainerContext, TrainingDataCollectionOptions, DefaultActionData>
    {
        public CollectTraningDataAction(Func<string, IActionExecutor> executorFactory, ILogger<CollectTraningDataAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = ModelTrainingExecutors.DefaultTrainingDataCollector.ToString();
            ActionType = ModelTrainingActions.TrainingDataCollection.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, TrainingDataCollectionOptions options, string message, object nextActionData)
        {
            if (jobContext.NeedTrain)
                return JobActionResult.NextAction(ModelTrainingActions.ModelTraining.ToString(), nextActionData, message);
            else
                return JobActionResult.Finish("It is not a good time to train the model.");
        }
    }
}

using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Data;
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
    public class UpdateTrainingHistoryAction : JobAction<ModelTrainerContext, TrainingStatusUpdatingOptions, TrainingStatusUpdatingActionData>
    {
        public UpdateTrainingHistoryAction(Func<string, IActionExecutor> executorFactory, ILogger<UpdateTrainingHistoryAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = ModelTrainingExecutors.DefaultTrainingHistoryUpdater.ToString();
            ActionType = ModelTrainingActions.TrainingHistoryUpdateing.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, TrainingStatusUpdatingOptions options, string message, object nextActionData)
        {
            return JobActionResult.Finish(message);
        }
    }
}
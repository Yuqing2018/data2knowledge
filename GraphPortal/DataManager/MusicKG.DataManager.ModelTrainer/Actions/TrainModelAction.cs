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
    public class TrainModelAction : JobAction<ModelTrainerContext, ModelTrainingOptions, ModelTrainingActionData>
    {
        public TrainModelAction(Func<string, IActionExecutor> executorFactory, ILogger<TrainModelAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = ModelTrainingExecutors.DefaultModelTrainer.ToString();
            ActionType = ModelTrainingActions.ModelTraining.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, ModelTrainingOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(ModelTrainingActions.ModelServing.ToString(), nextActionData, message);
        }
    }
}

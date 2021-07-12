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
    public class ServeModelAction : JobAction<ModelTrainerContext, ModelServingOptions, ModelServingActionData>
    {
        public ServeModelAction(Func<string, IActionExecutor> executorFactory, ILogger<ServeModelAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = ModelTrainingExecutors.DefaultModelServer.ToString();
            ActionType = ModelTrainingActions.ModelServing.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, ModelServingOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(ModelTrainingActions.TrainingHistoryUpdateing.ToString(), nextActionData, message);
        }
    }
}
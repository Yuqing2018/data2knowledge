using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models.Enums;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.DataManager.Translator.Actions
{
    /// <summary>
    /// Customerized job action.
    /// </summary>
    public class HandleDataAction : JobAction<DataHandlerContext, DataHandlingOptions, DataHandlingActionData>
    {
        public HandleDataAction(Func<string, IActionExecutor> executorFactory, ILogger<HandleDataAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = DataHandlingDefaultExecutors.DefaultDataHandler.ToString();
            ActionType = DataHandlingActions.DataHandling.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            DataHandlingOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(DataHandlingActions.TaskStatusUpdating.ToString(), nextActionData, message);
        }
    }
}

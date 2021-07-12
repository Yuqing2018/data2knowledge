using MusicKG.DataManager.Handler.Data;
using MusicKG.DataManager.Models.Enums;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.DataManager.Handler.Actions
{
    /// <summary>
    /// Customerized job action.
    /// </summary>
    public class UpdateTaskStatusAction : JobAction<DataHandlerContext, TaskStatusUpdatingOptions, TaskStatusUpdatingActionData>
    {
        public UpdateTaskStatusAction(Func<string, IActionExecutor> executorFactory, ILogger<UpdateTaskStatusAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = DataHandlingDefaultExecutors.DefaultTaskStatusUpdater.ToString();
            ActionType = DataHandlingActions.TaskStatusUpdating.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            TaskStatusUpdatingOptions options, string message, object nextActionData)
        {
            return JobActionResult.Finish($"Handling data from labeling result for workspace {jobContext.Parameters.WorkspaceId} finished.");
        }
    }
}

using TaskCreationOptions = MusicKG.DataManager.Translator.Options.TaskCreationOptions;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.DataManager.Translator.Actions
{
    public class CreateTaskAction : JobAction<DataTranslatorContext, TaskCreationOptions, TaskCreationActionData>
    {
        public CreateTaskAction(Func<string, IActionExecutor> executorFactory, ILogger<ConsumeDataAction> logger) 
            : base(executorFactory, logger)
        {
            defaultExecutorName = DataTranslationDefaultExecutors.DefaultTaskCreator.ToString();
            ActionType = DataTranslationActions.TaskCreation.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, TaskCreationOptions options, string message, object nextActionData)
        {
            if (jobContext.TimeRanges.Count == 0)
            {
                logger.LogActionInfo(action.ActionId, $"There is no more data being translated, the job will be finished.");

                return JobActionResult.Finish($"The job finished successfully.");
            }
            else
            {
                return JobActionResult.NextAction(
                    DataTranslationActions.DataCollection.ToString(),
                    new DefaultActionData(),
                    $"Saving data for source {jobContext.Parameters.DataSourceName} finished.");
            }
        }
    }
}

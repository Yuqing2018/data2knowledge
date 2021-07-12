using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.DataAccess.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.DataManager.Translator.Actions
{
    public class PreserveDataAction : JobAction<DataTranslatorContext, DataPreservationOptions, DataPreservationActionData>
    {
        public PreserveDataAction(Func<string, IActionExecutor> executorFactory, ILogger<PreserveDataAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = DataTranslationExecutors.DefaultDataPreserver.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, DataPreservationOptions options, string message, object nextActionData)
        {
            if (jobContext.CurrentTimeRanges.Count == 0)
            {
                logger.LogActionInfo(action.ActionId, $"There is no more data being translated, the job will be finished.");

                return JobActionResult.Finish("The job finished successfully.");
            }
            else
            {
                return JobActionResult.NextAction(
                    DataTranslationActions.DataCollection.ToString(),
                    new DefaultActionData(),
                    $"Preserve data for source {jobContext.Parameters.DataSourceName} finished.");
            }
        }
    }
}
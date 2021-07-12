using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.DataManager.Translator.Actions
{
    /// <summary>
    /// Customerized job action.
    /// </summary>
    public class ConsumeDataAction : JobAction<DataTranslatorContext, DataConsumptionOptions, DataConsumptionActionData>
    {
        public ConsumeDataAction(Func<string, IActionExecutor> executorFactory, ILogger<ConsumeDataAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = DataTranslationDefaultExecutors.DefaultDataConsumer.ToString();
            ActionType = DataTranslationActions.DataConsumption.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            DataConsumptionOptions options, string message, object nextActionData)
        {
            switch (options.Consumers)
            {
                case DataConsumers.LabelingTool:
                    return JobActionResult.NextAction(
                        DataTranslationActions.TaskCreation.ToString(),
                        nextActionData,
                        $"Consuming data by {DataConsumers.LabelingTool} for source {jobContext.Parameters.DataSourceName} finished.");
                case DataConsumers.Business:
                case DataConsumers.Model:
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
                default:
                    throw new NotSupportedException();
            }
        }
    }
}

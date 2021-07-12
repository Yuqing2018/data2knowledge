using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
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
    public class AnnotateDataAction : JobAction<DataTranslatorContext, DataAnnotationOptions, DataAnnotationActionData>
    {
        public AnnotateDataAction(Func<string, IActionExecutor> executorFactory, ILogger<AnnotateDataAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = DataTranslationDefaultExecutors.DefaultDataAnnotator.ToString();
            ActionType = DataTranslationActions.DataAnnotation.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            DataAnnotationOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(DataTranslationActions.DataConsumption.ToString(), nextActionData, message);
        }
    }
}

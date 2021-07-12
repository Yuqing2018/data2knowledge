using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
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
    public class ConstructDataAction : JobAction<DataTranslatorContext, DataConstructionOptions, DataConstructionActionData>
    {
        public ConstructDataAction(Func<string, IActionExecutor> executorFactory, ILogger<ConstructDataAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = DataTranslationExecutors.DefaultDataConstructor.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            DataConstructionOptions options, string message, object nextActionData)
        {
            if (!options.IgnoreManualAnnotation)
                return JobActionResult.NextAction(DataTranslationActions.DataNormalization.ToString(), nextActionData, message);
            else
                return JobActionResult.NextAction(DataTranslationActions.DataAnnotation.ToString(), nextActionData, message);
        }
    }
}

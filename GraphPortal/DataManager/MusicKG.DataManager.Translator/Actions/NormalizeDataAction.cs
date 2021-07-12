using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.DataAccess.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.DataManager.Translator.Actions
{
    public class NormalizeDataAction : JobAction<DataTranslatorContext, DataNormalizationOptions, DataNormalizationActionData>
    {
        public NormalizeDataAction(Func<string, IActionExecutor> executorFactory, ILogger<NormalizeDataAction> logger) : base(executorFactory, logger)
        {
            defaultExecutorName = DataTranslationDefaultExecutors.DefaultDataNormalizer.ToString();
            ActionType = DataTranslationActions.DataNormalization.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, DataNormalizationOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(DataTranslationActions.DataAnnotation.ToString(), nextActionData, message);
        }
    }
}
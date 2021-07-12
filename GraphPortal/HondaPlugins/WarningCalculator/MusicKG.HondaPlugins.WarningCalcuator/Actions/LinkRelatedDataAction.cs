using MusicKG.HondaPlugins.WarningCalculator.Contexts;
using MusicKG.HondaPlugins.WarningCalculator.Data;
using MusicKG.HondaPlugins.WarningCalculator.Options;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MusicKG.HondaPlugins.WarningCalculator.Actions
{
    public class LinkRelatedDataAction : JobAction<WarningCalculatorContext, RelatedDataLinkingOptions, RelatedDataLinkingActionData>
    {
        public LinkRelatedDataAction(Func<string, IActionExecutor> executorFactory, ILogger<LinkRelatedDataAction> logger)
           : base(executorFactory, logger)
        {
            ActionType = WarningCalculatorActions.LinkRelatedDataAction.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            RelatedDataLinkingOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(WarningCalculatorActions.CalculateWarningAction.ToString(),
                nextActionData, message);
        }
    }
}

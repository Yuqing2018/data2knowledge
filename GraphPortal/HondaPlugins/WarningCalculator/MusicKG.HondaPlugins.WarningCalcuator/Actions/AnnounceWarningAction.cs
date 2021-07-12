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
    public class AnnounceWarningAction : JobAction<WarningCalculatorContext, WarningAnnouncementOptions, WarningAnnouncementActionData>
    {
        public AnnounceWarningAction(Func<string, IActionExecutor> executorFactory, ILogger<AnnounceWarningAction> logger)
           : base(executorFactory, logger)
        {
            ActionType = WarningCalculatorActions.AnnounceWarningAction.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            WarningAnnouncementOptions options, string message, object nextActionData)
        {
            return JobActionResult.Finish(message);
        }
    }
}

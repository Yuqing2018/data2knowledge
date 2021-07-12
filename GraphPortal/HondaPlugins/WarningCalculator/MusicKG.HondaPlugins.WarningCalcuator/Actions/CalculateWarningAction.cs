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
    public class CalculateWarningAction : JobAction<WarningCalculatorContext, WarningCalculationOptions, WarningCalculationActionData>
    {
        public CalculateWarningAction(Func<string, IActionExecutor> executorFactory, ILogger<CalculateWarningAction> logger)
           : base(executorFactory, logger)
        {
            ActionType = WarningCalculatorActions.CalculateWarningAction.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action, 
            WarningCalculationOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(WarningCalculatorActions.SaveCalculationResultAction.ToString(),
                nextActionData, message);
        }
    }
}

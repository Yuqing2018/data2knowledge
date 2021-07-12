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
    public class SaveCalculationResultAction : JobAction<WarningCalculatorContext, CalculationResultSavingOptions, CalculationResultSavingActionData>
    {
        public SaveCalculationResultAction(Func<string, IActionExecutor> executorFactory, ILogger<SaveCalculationResultAction> logger)
           : base(executorFactory, logger)
        {
            ActionType = WarningCalculatorActions.SaveCalculationResultAction.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            CalculationResultSavingOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(WarningCalculatorActions.AnnounceWarningAction.ToString(),
                nextActionData, message);
        }
    }
}

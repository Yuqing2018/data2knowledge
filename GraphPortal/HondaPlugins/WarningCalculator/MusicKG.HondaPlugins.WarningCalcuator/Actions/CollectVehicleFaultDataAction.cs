using Microsoft.Extensions.Logging;
using MusicKG.HondaPlugins.WarningCalculator.Contexts;
using MusicKG.HondaPlugins.WarningCalculator.Options;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using System;

namespace MusicKG.HondaPlugins.WarningCalculator.Actions
{
    public class CollectVehicleFaultDataAction : JobAction<WarningCalculatorContext, VehicleFaultDataCollectionOptions, DefaultActionData>
    {
        public CollectVehicleFaultDataAction(Func<string, IActionExecutor> executorFactory, ILogger<CollectVehicleFaultDataAction> logger)
            : base(executorFactory, logger)
        {
            ActionType = WarningCalculatorActions.CollectVehicleFaultDataAction.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            VehicleFaultDataCollectionOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(WarningCalculatorActions.LinkRelatedDataAction.ToString(),
                nextActionData, message);
        }
    }
}

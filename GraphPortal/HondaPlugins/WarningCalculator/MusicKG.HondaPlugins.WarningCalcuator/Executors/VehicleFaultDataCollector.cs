using MusicKG.HondaPlugins.WarningCalculator.Contexts;
using MusicKG.HondaPlugins.WarningCalculator.Data;
using MusicKG.HondaPlugins.WarningCalculator.Options;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using MusicKG.Scheduler.Engine.Extensions;

namespace MusicKG.HondaPlugins.WarningCalculator.Executors
{
    public class VehicleFaultDataCollector : ActionExecutor<WarningCalculatorContext, VehicleFaultDataCollectionOptions, DefaultActionData, RelatedDataLinkingActionData>
    {
        private readonly IVehicleFaultDataService vehicleDataService;

        public VehicleFaultDataCollector(IVehicleFaultDataService vehicleDataService, ILogger<VehicleFaultDataCollector> logger) : base(logger)
        {
            this.vehicleDataService = vehicleDataService;
            ExecutorType = WarningCalculatorExecutors.VehicleFaultDataCollector.ToString();
        }

        protected async override Task<RelatedDataLinkingActionData> ExecuteInternalAsync(string actionId, 
            WarningCalculatorContext context, 
            VehicleFaultDataCollectionOptions options, 
            DefaultActionData data)
        {
            var task = context.Parameters;

            var result = await vehicleDataService.ListIdsAsync(task.CarModels, task.CarTypes, task.YearModels, null, null, null, context.TaskRunTime, false);

            logger.LogActionInfo(actionId, $"There are {result?.Count} vehicle fault data to be calculated.");

            return new RelatedDataLinkingActionData
            {
                VehicleFaultData = result
            };
        }

        protected async override Task RevertInternalAsync(string actionId, WarningCalculatorContext context)
        {
            //Do Nothing.
        }
    }
}

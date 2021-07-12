using MusicKG.HondaPlugins.WarningCalculator.Contexts;
using MusicKG.HondaPlugins.WarningCalculator.Data;
using MusicKG.HondaPlugins.WarningCalculator.Options;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.Services.Models;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.WarningCalculator.Executors
{
    public class RelatedDataLinker : ActionExecutor<WarningCalculatorContext, RelatedDataLinkingOptions, RelatedDataLinkingActionData, WarningCalculationActionData>
    {
        private readonly IVehicleFaultDataService vehicleFaultDataService;

        public RelatedDataLinker(IVehicleFaultDataService vehicleFaultDataService, ILogger<RelatedDataLinker> logger) : base(logger)
        {
            this.vehicleFaultDataService = vehicleFaultDataService;
            ExecutorType = WarningCalculatorExecutors.RelatedDataLinker.ToString();
        }

        protected async override Task<WarningCalculationActionData> ExecuteInternalAsync(string actionId,
            WarningCalculatorContext context,
            RelatedDataLinkingOptions options,
            RelatedDataLinkingActionData data)
        {
            if (data.VehicleFaultData == null || data.VehicleFaultData.Count == 0)
            {
                logger.LogActionInfo("There is no data to be linked.", actionId);
                return new WarningCalculationActionData { VehicleFaultData = null };
            }

            return new WarningCalculationActionData
            {
                VehicleFaultData = await vehicleFaultDataService.LinkRelatedDataAsyncBatch(data.VehicleFaultData)
            };
        }

        protected async override Task RevertInternalAsync(string actionId, WarningCalculatorContext context)
        {
            //Do Nothing.
        }
    }
}

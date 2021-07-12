using MusicKG.HondaPlugins.WarningCalculator.Contexts;
using MusicKG.HondaPlugins.WarningCalculator.Data;
using MusicKG.HondaPlugins.WarningCalculator.Options;
using MusicKG.HondaPlugins.WarningCalculator.Services;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Action;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

namespace MusicKG.HondaPlugins.WarningCalculator.Executors
{
    public class WarningRecordCalculator : ActionExecutor<WarningCalculatorContext, WarningCalculationOptions, WarningCalculationActionData, CalculationResultSavingActionData>
    {
        private readonly IWarningTaskCalculationService warningTaskCalculationService;
        private readonly IRiskWarningAIService riskWarningAIService;

        public WarningRecordCalculator(IWarningTaskCalculationService warningTaskCalculationService,
            IRiskWarningAIService riskWarningAIService,
            ILogger<WarningRecordCalculator> logger) : base(logger)
        {
            this.warningTaskCalculationService = warningTaskCalculationService;
            this.riskWarningAIService = riskWarningAIService;
            ExecutorType = WarningCalculatorExecutors.WarningRecordCalculator.ToString();
        }

        protected async override Task<CalculationResultSavingActionData> ExecuteInternalAsync(string actionId, 
            WarningCalculatorContext context,
            WarningCalculationOptions options,
            WarningCalculationActionData data)
        {
            if (data.VehicleFaultData == null || data.VehicleFaultData.Count == 0)
            {
                logger.LogActionInfo("There is no data to be calculated.", actionId);
                return new CalculationResultSavingActionData { WarningCalculationResult = null };
            }

            var records = await warningTaskCalculationService.CalculateWarningAsync(context.Task.Id, context.TaskRunTime, context.Parameters, data.VehicleFaultData);

            logger.LogActionInfo(actionId, "Finish warning calcualtion.");

            if (!options.IgnoreModel && context.Parameters.WarningIndex.Any(x => x.WarningType == DataAccess.Enums.WarningType.风险预警))
            {
                await riskWarningAIService.CalculateRiskWarnings(records, options.BatchSize);

                logger.LogActionInfo(actionId, "Finish ai risk warning calculation.");
            }
            
            return new CalculationResultSavingActionData
            {
                WarningCalculationResult = records
            };
        }

        protected async override Task RevertInternalAsync(string actionId, WarningCalculatorContext context)
        {
            //Do Nothing.
        }
    }
}

using MusicKG.HondaPlugins.WarningCalculator.Contexts;
using MusicKG.HondaPlugins.WarningCalculator.Data;
using MusicKG.HondaPlugins.WarningCalculator.Options;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Linq;

namespace MusicKG.HondaPlugins.WarningCalculator.Executors
{
    public class CalculationResultSaver : ActionExecutor<WarningCalculatorContext, CalculationResultSavingOptions, CalculationResultSavingActionData, WarningAnnouncementActionData>
    {
        private readonly IWarningRecordService warningRecordService;

        public CalculationResultSaver(IWarningRecordService warningRecordService, ILogger<CalculationResultSaver> logger) : base(logger)
        {
            this.warningRecordService = warningRecordService;
            ExecutorType = WarningCalculatorExecutors.CalculationResultSaver.ToString();
        }

        protected async override Task<WarningAnnouncementActionData> ExecuteInternalAsync(string actionId, 
            WarningCalculatorContext context,
            CalculationResultSavingOptions options,
            CalculationResultSavingActionData data)
        {
            if (data.WarningCalculationResult == null || data.WarningCalculationResult.Count == 0)
            {
                logger.LogActionInfo("There is no data to be saved.", actionId);
                return new WarningAnnouncementActionData { WarningRecords = null };
            }

            var (recordIds, detailIds) = await warningRecordService.SaveWarningRecordsAsync(context.Task.Id, data.WarningCalculationResult);

            context.SavedRecordIds = recordIds;
            context.SavedDetailIds = detailIds;

            return new WarningAnnouncementActionData { WarningRecords = data.WarningCalculationResult };
        }

        protected async override Task RevertInternalAsync(string actionId, WarningCalculatorContext context)
        {
            if (context.SavedRecordIds.Any())
                await warningRecordService.RemoveWarningRecords(context.SavedRecordIds);

            if (context.SavedDetailIds.Any())
                await warningRecordService.RemoveWarningDetails(context.SavedDetailIds);
        }
    }
}

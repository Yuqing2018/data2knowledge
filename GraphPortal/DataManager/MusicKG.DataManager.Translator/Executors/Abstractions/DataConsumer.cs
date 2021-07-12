using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.DataManager.Translator.Executors.Abstractions
{
    public abstract class DataConsumer<TAnnotationItem> : ActionExecutor<DataTranslatorContext, DataConsumptionOptions, DataConsumptionActionData, TaskCreationActionData> where TAnnotationItem : AnnotationItemModel
    {
        public DataConsumer(ILogger<DataConsumer<TAnnotationItem>> logger) : base(logger)
        {
        }

        protected async override Task<TaskCreationActionData> ExecuteInternalAsync(string actionId,
            DataTranslatorContext context,
            DataConsumptionOptions options,
            DataConsumptionActionData data)
        {
            if (options.Ignore || data == null || data.Data == null || context.DataCount <= 0)
            {
                logger.LogActionInfo("There is no data to be consumed.", actionId);
                return new TaskCreationActionData { Documents = null };
            }

            return ConsumeData(actionId, context, options, data.Data?.Cast<TAnnotationItem>());
        }

        protected abstract TaskCreationActionData ConsumeData(string actionId, DataTranslatorContext context, DataConsumptionOptions options, IEnumerable<TAnnotationItem> items);
    }
}

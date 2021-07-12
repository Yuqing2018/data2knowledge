using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.DataManager.Translator.Executors.Abstractions
{
    public abstract class DataAnnotator<TAnnotatinData> : ActionExecutor<DataTranslatorContext, DataAnnotationOptions, DataAnnotationActionData, DataConsumptionActionData> where TAnnotatinData : AnnotationItemModel
    {
        public DataAnnotator(ILogger<DataAnnotator<TAnnotatinData>> logger) : base(logger)
        {
        }

        protected async override Task<DataConsumptionActionData> ExecuteInternalAsync(string actionId, 
            DataTranslatorContext context, 
            DataAnnotationOptions options,
            DataAnnotationActionData data)
        {
            if (data == null || data.RawData == null || context.DataCount <= 0)
            {
                logger.LogActionInfo("There is no data to be annotated.", actionId);
                return new DataConsumptionActionData { Data = null };
            }

            if (options.Ignore)
                return new DataConsumptionActionData { Data = data.RawData?.SelectMany(d => d) };

            var result = AnnotateData(actionId, context, options, data.RawData?.Select(d => d.Cast<TAnnotatinData>()?.ToList()));

            return new DataConsumptionActionData
            {
                Data = result?.Cast<AnnotationItemModel>()
            };
        }

        protected async override Task RevertInternalAsync(string actionId, DataTranslatorContext context)
        {
            // Nothing to revert.
        }

        protected abstract IEnumerable<TAnnotatinData> AnnotateData(string actionId, DataTranslatorContext context, DataAnnotationOptions options, IEnumerable<List<TAnnotatinData>> data);
    }
}

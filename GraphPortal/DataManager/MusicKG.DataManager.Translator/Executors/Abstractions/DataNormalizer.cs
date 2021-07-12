using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicKG.DataManager.Translator.Executors.Abstractions
{
    public abstract class DataNormalizer<TAnnotationItem> : ActionExecutor<DataTranslatorContext, DataNormalizationOptions, DataNormalizationActionData, DataAnnotationActionData> where TAnnotationItem : AnnotationItemModel
    {
        public DataNormalizer(ILogger<DataNormalizer<TAnnotationItem>> logger) : base(logger)
        {
        }

        protected async override Task<DataAnnotationActionData> ExecuteInternalAsync(string actionId, 
            DataTranslatorContext context,
            DataNormalizationOptions options,
            DataNormalizationActionData data)
        {
            if (options.Ignore || data == null || data.RawData == null || context.DataCount <= 0)
            {
                logger.LogActionInfo("There is no data to be constructed.", actionId);
                return new DataAnnotationActionData { RawData = null };
            }

            var result = NormalizeData(actionId, context, options, data.RawData);

            return new DataAnnotationActionData
            {
                RawData = result?.Select(r => r.Cast<AnnotationItemModel>()?.ToList())
            };
        }

        protected async override Task RevertInternalAsync(string actionId, DataTranslatorContext context)
        {
            // Nothing to revert.
        }

        protected abstract IEnumerable<List<TAnnotationItem>> NormalizeData(string actionId,
            DataTranslatorContext context,
            DataNormalizationOptions options,
            IEnumerable<List<Dictionary<string, object>>> items);
    }
}

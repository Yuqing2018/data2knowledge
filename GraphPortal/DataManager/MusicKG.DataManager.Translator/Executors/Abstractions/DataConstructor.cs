using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Models;
using MusicKG.DataManager.Translator.Options;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.DataManager.Translator.Executors.Abstractions
{
    public abstract class DataConstructor<TAnnotatinData> : ActionExecutor<DataTranslatorContext, DataConstructionOptions, DataConstructionActionData, DataNormalizationActionData> where TAnnotatinData : AnnotationItemModel
    {
        public DataConstructor(ILogger<DataConstructor<TAnnotatinData>> logger) : base(logger)
        {
        }

        protected async override Task<DataNormalizationActionData> ExecuteInternalAsync(string actionId, 
            DataTranslatorContext context, 
            DataConstructionOptions options, 
            DataConstructionActionData data)
        {
            if (data == null || data.RawData == null)
            {
                logger.LogActionInfo("There is no data to be constructed.", actionId);
                return new DataNormalizationActionData { RawData = null };
            }

            var result = ConstructData(actionId, context, options, data.RawData);

            return new DataNormalizationActionData
            {
                RawData = result?.Cast<AnnotationItemModel>()
            };
        }

        protected abstract IEnumerable<TAnnotatinData> ConstructData(string actionId,
            DataTranslatorContext context,
            DataConstructionOptions options,
            IEnumerable<Dictionary<string, object>> data);
    }
}

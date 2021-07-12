using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicKG.DataManager.Translator.Executors.Abstractions
{
    public abstract class DataCollector : ActionExecutor<DataTranslatorContext, DataCollectionOptions, DefaultActionData, DataNormalizationActionData>
    {
        public DataCollector(ILogger<DataCollector> logger) : base(logger)
        {
        }

        protected async override Task<DataNormalizationActionData> ExecuteInternalAsync(string actionId, 
            DataTranslatorContext context, 
            DataCollectionOptions options, 
            DefaultActionData data)
        {
            if (context.TimeRanges?.Count == 0)
            {
                logger.LogActionInfo(actionId, $"The time range is empty, indicate there is no data to be translated.");

                return new DataNormalizationActionData { RawData = null };
            }

            var timeRange = context.TimeRanges.Dequeue();

            context.CurrentTimeRange = timeRange;

            context.DataCount = await CountDataAsync(actionId, context, options, timeRange.Start, timeRange.End);

            if (context.DataCount == 0)
                return new DataNormalizationActionData { RawData = null };

            logger.LogActionInfo(actionId, $"There are {context.DataCount} items to be translated.");

            var result = CollectData(actionId, context, options, timeRange.Start, timeRange.End);

            return new DataNormalizationActionData { RawData = result };
        }

        protected async override Task RevertInternalAsync(string actionId, DataTranslatorContext context)
        {
            var timeRanges = context.TimeRanges;
            var currentTimeRange = context.CurrentTimeRange;
            if (currentTimeRange != null)
            {
                context.TimeRanges = new Queue<TimeRange>();
                context.TimeRanges.Enqueue(currentTimeRange);
                foreach (var timeRange in timeRanges)
                    context.TimeRanges.Enqueue(timeRange);
            }
        }

        protected abstract Task<long> CountDataAsync(string actionId,
            DataTranslatorContext context, DataCollectionOptions options,
            DateTime start, DateTime end);

        protected abstract IEnumerable<List<Dictionary<string, object>>> CollectData(string actionId,
            DataTranslatorContext context, DataCollectionOptions options,
            DateTime start, DateTime end);
    }
}
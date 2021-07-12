using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Options;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.PredictWrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaDataAnnotator : DataAnnotator<AnnotationDocumentItem>
    {
        private readonly IVehiclePredictor predictor;
        private readonly IHondaMongoDbContext hondaDbContext;

        public HondaDataAnnotator(
            IVehiclePredictor predictor,
            IHondaMongoDbContext hondaDbContext,
            ILogger<HondaDataAnnotator> logger) : base(logger)
        {
            this.hondaDbContext = hondaDbContext;
            this.predictor = predictor;
            ExecutorType = HondaExecutors.HondaDataAnnotator.ToString();
        }

        protected override IEnumerable<AnnotationDocumentItem> AnnotateData(string actionId, 
            DataTranslatorContext context, 
            DataAnnotationOptions options, 
            IEnumerable<List<AnnotationDocumentItem>> data)
        {
            var result = data?.SelectMany(items =>
            {
                predictor.Initialize();

                List<AnnotationDocumentItem> predictResult;

                if (!string.IsNullOrWhiteSpace(context.Parameters.DataSourceName))
                {
                    predictResult = PredictBatch(context.Parameters.DataSourceName, items, options.BatchSize);
                }
                else
                {
                    predictResult = items.GroupBy(i => i.DataSourceName).SelectMany(items => PredictBatch(items.Key, items.ToList(), options.BatchSize)).ToList();
                }

                predictor.Close();

                return predictResult;
            });

            return result;
        }

        private List<AnnotationDocumentItem> PredictBatch(string dataSourceName, List<AnnotationDocumentItem> items, int batchSize)
        {
            var predictPartName = dataSourceName != DataSource.MQI.ToString();

            var requestCount = (int)Math.Ceiling((decimal)items.Count / batchSize);

            var predictResults = Enumerable.Range(0, requestCount).SelectMany(index =>
            {
                var itemBatch = items?.Skip(index * batchSize)?.Take(batchSize)?.ToList();

                return predictor.PredictBatchAsync(itemBatch, predictPartName, true).GetAwaiter().GetResult();
            })?.ToList();

            return items.Select(item =>
            {
                var predictResult = predictResults.FirstOrDefault(r => r.ItemId == item.Id);

                if (predictResult != null)
                {
                    PredictResult partName;

                    if (predictPartName)
                    {
                        partName = predictResult.PartName;
                    }
                    else
                    {
                        partName = item.OriginalResult.PartName;
                    }

                    item.ModelResult = new MarkResult
                    {
                        IsAddForTraining = false,
                        PartName = partName,
                        Syndrome = predictResult.Syndrome
                    };

                    item.Result = item.ModelResult;
                }

                return item;
            })?.ToList();
        }
    }
}

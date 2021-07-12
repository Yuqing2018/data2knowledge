using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Net.Http;

namespace MusicKG.DataManager.Translator.Executors
{
    public class DefaultLabelingToolDataConsumer : LabelingToolDataConsumer<DefaultAnnotationItem>
    {
        protected const string joinString = "**Join**";

        public DefaultLabelingToolDataConsumer(ILogger<DefaultLabelingToolDataConsumer> logger) : base(logger)
        {
            ExecutorType = DataTranslationDefaultExecutors.DefaultLabelingToolDataConsumer.ToString();
        }

        protected override IEnumerable<DataPreservationModel> ConvertDataToDocuments(DataTranslatorContext context,
            DataConsumptionOptions options,
            IEnumerable<DefaultAnnotationItem> data)
        {
            var settings = context.Parameters.DataConsumerSettings;

            var itemsPreDocument = context.Parameters.DataConsumerSettings.ItemsPerDocument;

            var grouppedData = data?.GroupBy(d =>
            {
                var tags = settings.TagFeatures.Intersect(d.AnnotationFeatures?.Keys)?.Select(key => d.AnnotationFeatures[key]?.ToString())?.ToList();

                if (tags == null)
                    tags = new List<string>();

                tags.Insert(0, context.Parameters.DataSourceName);

                return string.Join(joinString, tags);
            });

            return grouppedData?.SelectMany((d, index) =>
            {
                var tags = d.Key.Split(joinString).ToList();

                var totalDocumentCount = (int)Math.Ceiling((decimal)d.Count() / itemsPreDocument);

                return Enumerable.Range(0, totalDocumentCount).Select(i =>
                {
                    var items = d.Skip(i * itemsPreDocument).Take(itemsPreDocument);

                    return new DataPreservationModel
                    {
                        FileName = $"{string.Join("-", tags)}_{DateTime.UtcNow.ToString(settings.DateTimeFormat)}_{index + 1}",
                        ContentType = settings.ContentType,
                        ItemCount = d.Count(),
                        Tags = tags,
                        Content = ConvertDataToBytes(context, options, tags, items)
                    };
                });
            });
        }
    }
}

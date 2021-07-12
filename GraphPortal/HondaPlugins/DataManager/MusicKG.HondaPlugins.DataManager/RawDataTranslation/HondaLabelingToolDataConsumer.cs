using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess.DataModels.Raws;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.DataAccess.Resources;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Models;
using MusicKG.WebApi.ClientWrapper;
using MusicKG.HondaPlugins.DataAccess;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaLabelingToolDataConsumer : LabelingToolDataConsumer<AnnotationDocumentItem>
    {
        private const string joinString = "**Join**";
        private const string partNameDictionaryName = "零件名";

        private readonly IHondaMongoDbContext dbContext;
        private readonly IRawDbContext rawDbContext;

        public HondaLabelingToolDataConsumer(IRawDbContext rawDbContext, IHondaMongoDbContext dbContext, ILogger<HondaLabelingToolDataConsumer> logger) 
            : base(logger)
        {
            this.dbContext = dbContext;
            this.rawDbContext = rawDbContext;
            ExecutorType = HondaExecutors.HondaLabelingToolDataConsumer.ToString();
        }

        protected override IEnumerable<DataPreservationModel> ConvertDataToDocuments(DataTranslatorContext context,
            DataConsumptionOptions options, IEnumerable<AnnotationDocumentItem> data)
        {
            var httpClient = context.HttpClient;
            var token = context.Token;

            var settings = context.Parameters.DataConsumerSettings;

            var itemsPreDocument = context.Parameters.DataConsumerSettings.ItemsPerDocument;

            var dicts = DictionaryWrapper.ListAsync(httpClient, context.ServiceUrl, context.Parameters.WorkspaceId, token).GetAwaiter().GetResult();

            var dicId = dicts?.FirstOrDefault(d => d.Name == partNameDictionaryName)?.Id;

            var grouppedData = data?.GroupBy(row => $"{Columns.ResourceManager.GetString(context.Parameters.DataSourceName)}{joinString}{row.CarModel}");

            return grouppedData?.SelectMany((d, index) =>
            {
                var tags = d.Key.Split(joinString).ToList();

                var totalDocumentCount = (int)Math.Ceiling((decimal)d.Count() / itemsPreDocument);

                return Enumerable.Range(0, totalDocumentCount).Select(i =>
                {
                    var items = d.Skip(i * itemsPreDocument).Take(itemsPreDocument);

                    if (context.Parameters.DataSourceName == DataAccess.Enums.DataSource.MQI.ToString())
                        UpdateBusinessData(context.ServiceUrl, context.Parameters.WorkspaceId, dicId, items, httpClient, token);

                    return new DataPreservationModel
                    {
                        FileName = $"{string.Join("-", tags)}_{DateTime.UtcNow.ToString(settings.DateTimeFormat)}_{i + 1}",
                        ContentType = settings.ContentType,
                        ItemCount = items.Count(),
                        Tags = tags,
                        Content = ConvertDataToBytes(context, options, tags, items)
                    };
                });
            });
        }

        private void UpdateBusinessData(string serviceUrl, string workspaceId, string dictId, IEnumerable<AnnotationDocumentItem> items, HttpClient httpClient, string token)
        {
            if (items != null)
            {
                var partNames = new HashSet<string>();

                foreach (var item in items)
                {
                    AddPartName(item);

                    if (!string.IsNullOrWhiteSpace(item.OriginalResult?.PartName?.Value))
                        partNames.Add(item.OriginalResult.PartName.Value);
                    else
                        partNames.Add(ConstantSettings.UnknownString);
                }

                DictionaryWrapper.UpdateVocabsAsync(httpClient, serviceUrl,
                    workspaceId, dictId,
                    partNames, token).GetAwaiter().GetResult();
            }
        }

        private void AddPartName(AnnotationDocumentItem item)
        {
            if (string.IsNullOrWhiteSpace(item.OriginalResult?.PartName?.Value))
                return;

            var exists = dbContext.Parts.AsQueryable().Any(x => x.No == item.PartNo);

            var builder = Builders<VehiclePartDataModel>.Update;

            if (!exists)
            {
                dbContext.Parts.InsertOne(new VehiclePartDataModel
                {
                    No = item.PartNo,
                    Names = new List<string> { item.OriginalResult.PartName.Value },
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
    }
}

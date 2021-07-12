using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaBusinessDataConsumer : DataConsumer<AnnotationDocumentItem>
    {
        private readonly IHondaMongoDbContext dbContext;

        public HondaBusinessDataConsumer(IHondaMongoDbContext dbContext, ILogger<HondaBusinessDataConsumer> logger) : base(logger)
        {
            this.dbContext = dbContext;
            ExecutorType = HondaExecutors.HondaBusinessDataConsumer.ToString();
        }

        protected override TaskCreationActionData ConsumeData(string actionId,
            DataTranslatorContext context,
            DataConsumptionOptions options,
            IEnumerable<AnnotationDocumentItem> data)
        {
            if (!Enum.TryParse<DataSource>(context.Parameters.DataSourceName, out var dataSource))
                dataSource = DataSource.MQI;

            switch (options.Consumers)
            {
                case DataConsumers.Business:
                    SaveResult(context.DataCount, data?.Select(d => d.ToVehicleFaultDataModel(dataSource, context.TaskRunTime)));
                    break;
                default:
                    break;
            }

            return new TaskCreationActionData { Documents = null };
        }

        protected async override Task RevertInternalAsync(string actionId, DataTranslatorContext context)
        {
            //No need to be reverted because the action is upsert.
        }

        private void SaveResult(long dataCount, IEnumerable<VehicleFaultDataModel> dataModels)
        {
            if (dataModels == null || dataCount <= 0)
                return;

            var parts = dbContext.Parts.AsQueryable().Select(p => new { No = p.No, Names = p.Names }).ToList();

            foreach (var dataModel in dataModels)
            {
                var partNo = string.Join(",", parts?.Where(p => p.Names.Contains(dataModel.PartName))?.Select(p => p.No));

                if (!string.IsNullOrWhiteSpace(partNo))
                    dataModel.PartNo = partNo;

                var filter = Builders<VehicleFaultDataModel>.Filter;

                dbContext.VehicleFault.ReplaceOne(filter.Eq(t => t.RawId, dataModel.RawId), dataModel, new ReplaceOptions
                {
                    IsUpsert = true
                });
            }
        }
    }
}

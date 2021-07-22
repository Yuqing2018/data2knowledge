using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaVehicleFaultDataConsumer : DataConsumer<AnnotationDocumentItem>
    {
        private readonly IHondaMongoDbContext dbContext;

        public HondaVehicleFaultDataConsumer(IHondaMongoDbContext dbContext, ILogger<HondaVehicleFaultDataConsumer> logger) : base(logger)
        {
            this.dbContext = dbContext;
            ExecutorType = HondaExecutors.HondaVehicleFaultDataConsumer.ToString();
        }

        protected override TaskCreationActionData ConsumeData(string actionId,
            DataTranslatorContext context,
            DataConsumptionOptions options,
            IEnumerable<AnnotationDocumentItem> data)
        {
            switch (options.Consumers)
            {
                case DataConsumers.Business:
                    SaveResult(actionId, context.DataCount, data);
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

        private void SaveResult(string actionId, long dataCount, IEnumerable<AnnotationDocumentItem> dataModels)
        {
            if (dataModels == null || dataCount <= 0)
                return;

            var parts = dbContext.Parts.AsQueryable().Select(p => new { No = p.No, Name = p.Names.FirstOrDefault() }).ToList();

            foreach (var dataModel in dataModels)
            {
                var partName = dataModel.Result?.PartName?.Value;

                if (!string.IsNullOrWhiteSpace(partName))
                {
                    var partNo = parts?.FirstOrDefault(p => p.Name == partName)?.No;

                    var filter = Builders<VehicleFaultDataModel>.Filter
                        .Eq(v => v.RawId, dataModel.RawId);

                    var updateBuilder = Builders<VehicleFaultDataModel>.Update;

                    var update = updateBuilder.Set(v => v.PartName, partName)
                        .Set(v => v.Syndrome, dataModel.Result.Syndrome.Value);

                    if (!string.IsNullOrWhiteSpace(partNo))
                        update = update.Set(v => v.PartNo, partNo);

                    dbContext.VehicleFault.UpdateOne(filter, update);
                }
                else
                {
                    logger.LogActionWarning(actionId, "Part name is empty.");
                }
            }
        }
    }
}

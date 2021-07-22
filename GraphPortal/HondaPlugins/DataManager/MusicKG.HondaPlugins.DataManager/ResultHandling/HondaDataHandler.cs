using MusicKG.HondaPlugins.ModelTrainer.Extensions;
using MusicKG.HondaPlugins.DataManager.Helpers;
using MusicKG.DataManager.Translator.Executors.Abstractions;
using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using MusicKG.HondaPlugins.DataAccess.Settings;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Linq;

namespace MusicKG.HondaPlugins.DataManager.Executors
{
    public class HondaDataHandler : DataHandler<AnnotationDocumentItem>
    {
        private readonly IHondaMongoDbContext dbContext;

        public HondaDataHandler(IHondaMongoDbContext dbContext, ILogger<HondaDataHandler> logger) : base(logger)
        {
            this.dbContext = dbContext;
            ExecutorType = HondaExecutors.HondaDataHandler.ToString();
        }

        protected override void HandleDataAsBusiness(string actionId, 
            DataHandlerContext context, 
            DataHandlingOptions options, 
            AnnotationDocumentItem annotationItem)
        {
            var dataSource = Enum.Parse<DataSource>(annotationItem.DataSourceName);

            var filter = Builders<VehicleFaultDataModel>.Filter;

            var dataModel = annotationItem.ToVehicleFaultDataModel(dataSource, context.TaskRunTime);

            var parts = dbContext.Parts.AsQueryable()
                .Where(p => p.Names.Contains(dataModel.PartName))
                .ToList();

            var partNo = string.Join(",", parts?.Select(p => p.No));

            if (string.IsNullOrWhiteSpace(partNo))
                dataModel.PartNo = ConstantSettings.UnknownString;
            else
                dataModel.PartNo = partNo;

            VehicleDataHelper.UpsertVehicleDataAsync(dbContext, dataModel).GetAwaiter().GetResult();
        }

        protected override void HandleDataAsModelTraning(string actionId, DataHandlerContext context, DataHandlingOptions options, AnnotationDocumentItem annotationItem)
        {
            if (!NeedAddToTraining(annotationItem))
                return;

            var dataSource = Enum.Parse<DataSource>(annotationItem.DataSourceName);

            var filter = Builders<ModelTrainingData>.Filter;

            var syndomeId = annotationItem.Result?.Syndrome?.Value;

            if (!string.IsNullOrWhiteSpace(syndomeId))
            {
                var syndomeValue = dbContext.Syndromes.AsQueryable().FirstOrDefault(s => s.Id == new ObjectId(syndomeId));

                annotationItem.Result.Syndrome.Id = syndomeValue?.Name;

                var dataModel = annotationItem.ToModelTrainingData(dataSource, context.TaskRunTime);

                dbContext.TrainingData.ReplaceOne(filter.Eq(t => t.Id, dataModel.Id), dataModel, new ReplaceOptions
                {
                    IsUpsert = true
                });
            }
        }

        private bool NeedAddToTraining(AnnotationDocumentItem annotationItem)
        {
            return annotationItem.Result != null && (annotationItem.Result.IsAddForTraining || annotationItem.DataSourceName == DataSource.MQI.ToString());
        }
    }
}

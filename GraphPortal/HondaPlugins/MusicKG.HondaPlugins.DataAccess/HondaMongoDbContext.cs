using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.DataAccess.Models;
using MusicKG.DataAccess.Constants;
using MusicKG.DataManager.Models;
using MongoDB.Driver;
using System;

namespace MusicKG.HondaPlugins.DataAccess
{
    public class HondaMongoDbContext : IHondaMongoDbContext
    {
        public HondaMongoDbContext(HondaMongoDbSettings options)
        {
            var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);

            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);

            Client = new MongoClient(settings);
            Database = Client.GetDatabase(options.Database);

            GetCollections();

            EnsureIndexes();
        }

        public IMongoClient Client { get; private set; }

        public IMongoDatabase Database { get; private set; }

        public IMongoCollection<VehicleFaultDataModel> VehicleFault { get; private set; }

        public IMongoCollection<AIRiskWarningLevelMappingDataModel> AIRiskWarningMapping { get; private set; }

        public IMongoCollection<WarningIndexDataModel> WarningIndex { get; private set; }

        public IMongoCollection<WarningRecordDataModel> WarningRecord { get; private set; }

        public IMongoCollection<WarningTaskDetailDataModel> WarningTaskDetail { get; private set; }

        public IMongoCollection<SyndromeDataModel> Syndromes { get; private set; }

        public IMongoCollection<VehiclePartDataModel> Parts { get; private set; }

        public IMongoCollection<IgnoredVehicle> IgnoredVehicle { get; private set; }

        public IMongoCollection<ModelTrainingData> TrainingData { get; private set; }

        public IMongoCollection<ModelTrainingHistory> TrainingHistory { get; private set; }

        public IMongoCollection<UserDataModel> Users { get; private set; }

        private void GetCollections()
        {
            VehicleFault = Database.GetCollection<VehicleFaultDataModel>($"Honda_{nameof(VehicleFault)}");
            WarningIndex = Database.GetCollection<WarningIndexDataModel>($"Honda_Warning_{nameof(WarningIndex)}");
            WarningRecord = Database.GetCollection<WarningRecordDataModel>($"Honda_Warning_{nameof(WarningRecord)}");
            WarningTaskDetail = Database.GetCollection<WarningTaskDetailDataModel>($"Honda_Warning_{nameof(WarningTaskDetail)}");
            AIRiskWarningMapping = Database.GetCollection<AIRiskWarningLevelMappingDataModel>($"Honda_{nameof(AIRiskWarningMapping)}");

            Syndromes = Database.GetCollection<SyndromeDataModel>($"Honda_{nameof(Syndromes)}");
            Parts = Database.GetCollection<VehiclePartDataModel>($"Honda_{nameof(Parts)}");

            IgnoredVehicle = Database.GetCollection<IgnoredVehicle>($"Honda_{nameof(IgnoredVehicle)}");

            TrainingData = Database.GetCollection<ModelTrainingData>($"Honda_{nameof(TrainingData)}");
            TrainingHistory = Database.GetCollection<ModelTrainingHistory>($"Honda_{nameof(TrainingHistory)}");

            Users = Database.GetCollection<UserDataModel>(CollectionNames.User);
        }

        private void EnsureIndexes()
        {
            TrainingHistory.Indexes.CreateOne(new CreateIndexModel<ModelTrainingHistory>(Builders<ModelTrainingHistory>.IndexKeys.Ascending(_ => _.ModelName),
                new CreateIndexOptions { Background = true, Unique = true }));

            Parts.Indexes.CreateMany(new[]
            {
                 new CreateIndexModel<VehiclePartDataModel>(Builders<VehiclePartDataModel>.IndexKeys.Ascending(_ => _.No),
                    new CreateIndexOptions { Background = true, Unique = true })
            });

            VehicleFault.Indexes.CreateMany(new[]
            {
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Ascending(_ => _.RawId),
                    new CreateIndexOptions { Background = true, Unique = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Descending(_ => _.CarType),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Descending(_ => _.CarModel),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Descending(_ => _.ModelYear),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Ascending(_ => _.DataSource),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Descending(_ => _.PartName),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Descending(_ => _.Syndrome),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Descending(_ => _.ProductionDate),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Descending(_ => _.FaultDate),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<VehicleFaultDataModel>(Builders<VehicleFaultDataModel>.IndexKeys.Descending(_ => _.InitialRegistDate),
                    new CreateIndexOptions { Background = true })
            });

            WarningRecord.Indexes.CreateOne(new CreateIndexModel<WarningRecordDataModel>(Builders<WarningRecordDataModel>.IndexKeys.Descending(_ => _.WarningTime),
                new CreateIndexOptions { Background = true }));

            WarningTaskDetail.Indexes.CreateMany(new[] {
                new CreateIndexModel<WarningTaskDetailDataModel>(Builders<WarningTaskDetailDataModel>.IndexKeys.Ascending(_ => _.TaskId),
                    new CreateIndexOptions { Background = true }),
                new CreateIndexModel<WarningTaskDetailDataModel>(Builders<WarningTaskDetailDataModel>.IndexKeys.Descending(_ => _.FocusedDate),
                    new CreateIndexOptions { Background = true })
                });

            Syndromes.Indexes.CreateOne(new CreateIndexModel<SyndromeDataModel>(Builders<SyndromeDataModel>.IndexKeys.Ascending(_ => _.Name),
                new CreateIndexOptions { Background = true }));

            Parts.Indexes.CreateOne(new CreateIndexModel<VehiclePartDataModel>(Builders<VehiclePartDataModel>.IndexKeys.Ascending(_ => _.No),
                new CreateIndexOptions { Background = true, Unique = true }));
        }
    }
}

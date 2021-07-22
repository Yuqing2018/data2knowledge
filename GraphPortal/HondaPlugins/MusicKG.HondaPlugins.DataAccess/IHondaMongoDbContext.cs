using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.DataModels.Labeling;
using MusicKG.DataManager.Models;
using MongoDB.Driver;

namespace MusicKG.HondaPlugins.DataAccess
{
    public interface IHondaMongoDbContext
    {
        IMongoClient Client { get; }

        IMongoDatabase Database { get; }

        #region Business Data

        IMongoCollection<VehicleFaultDataModel> VehicleFault { get; }

        IMongoCollection<WarningIndexDataModel> WarningIndex { get; }

        IMongoCollection<WarningRecordDataModel> WarningRecord { get; }

        IMongoCollection<WarningTaskDetailDataModel> WarningTaskDetail { get; }

        IMongoCollection<SyndromeDataModel> Syndromes { get; }

        IMongoCollection<VehiclePartDataModel> Parts { get; }

        IMongoCollection<AIRiskWarningLevelMappingDataModel> AIRiskWarningMapping { get; }

        #endregion

        #region Models

        public IMongoCollection<IgnoredVehicle> IgnoredVehicle { get; }

        public IMongoCollection<ModelTrainingData> TrainingData { get; }

        public IMongoCollection<ModelTrainingHistory> TrainingHistory { get; }

        #endregion
    }
}

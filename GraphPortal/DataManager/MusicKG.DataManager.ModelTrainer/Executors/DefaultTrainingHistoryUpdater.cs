using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Settings;
using MusicKG.DataManager.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace MusicKG.DataManager.ModelTrainer.Executors
{
    public class DefaultTrainingHistoryUpdater : TrainingHistoryUpdater
    {
        private readonly IMongoCollection<ModelTrainingHistory> trainingHistoryCollection;

        public DefaultTrainingHistoryUpdater(DefaultModelTrainerDbSettings dbSettings, ILogger<DefaultTrainingHistoryUpdater> logger) 
            : base(logger)
        {
            var mongoSettings = MongoClientSettings.FromConnectionString(dbSettings.ConnectionString);

            mongoSettings.MaxConnectionIdleTime = TimeSpan.FromSeconds(int.MaxValue);

            var client = new MongoClient(mongoSettings);

            var database = client.GetDatabase(dbSettings.Database);

            trainingHistoryCollection = database.GetCollection<ModelTrainingHistory>(dbSettings.TrainingHistoryTableName);
        }

        protected async override Task RevertTrainingStatusAsync(string actionId, string modelName, int version)
        {
            await trainingHistoryCollection.UpdateOneAsync(h => h.ModelName == modelName,
                Builders<ModelTrainingHistory>.Update.PullFilter(d => d.ModelVersions, v => v.Version == version));
        }

        protected async override Task UpdateTrainingStatusAsync(string actionId, ModelTrainerContext context, TrainingStatusUpdatingOptions options, int version)
        {
            var modelName = context.ModelName;

            var history = await trainingHistoryCollection.AsQueryable()
                .FirstOrDefaultAsync(h => h.ModelName == modelName);

            if (history == null)
            {
                await trainingHistoryCollection.InsertOneAsync(new ModelTrainingHistory
                {
                    ModelName = modelName,
                    ModelVersions = new System.Collections.Generic.List<ModelVersion>
                    {
                        new ModelVersion
                        {
                            Version = version,
                            TrainedAt = context.TaskRunTime
                        }
                    }
                });
            }
            else
            {
                if (history.ModelVersions == null)
                    history.ModelVersions = new System.Collections.Generic.List<ModelVersion>();

                var tmpVersion = history.ModelVersions.FirstOrDefault(v => v.Version == version);

                if (tmpVersion == null)
                    history.ModelVersions.Add(new ModelVersion
                    {
                        Version = version,
                        TrainedAt = context.TaskRunTime
                    });
                else
                    tmpVersion.TrainedAt = context.TaskRunTime;

                await trainingHistoryCollection.ReplaceOneAsync(h => h.ModelName == modelName, history);
            }
        }
    }
}

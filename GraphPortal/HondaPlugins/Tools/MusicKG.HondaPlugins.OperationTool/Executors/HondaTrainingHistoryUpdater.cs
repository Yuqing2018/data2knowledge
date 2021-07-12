using MusicKG.DataManager.Models;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.HondaPlugins.OperationTool.Settings;
using MusicKG.HondaPlugins.DataAccess;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.OperationTool.Executors
{
    public class HondaTrainingHistoryUpdater : TrainingHistoryUpdater
    {
        private readonly IHondaMongoDbContext dbContext;

        public HondaTrainingHistoryUpdater(IHondaMongoDbContext dbContext, ILogger<HondaTrainingHistoryUpdater> logger)
            : base(logger)
        {
            this.dbContext = dbContext;
            ExecutorType = HondaModelTrainerExecutors.HondaTrainingHistoryUpdater.ToString();
        }

        protected async override Task RevertTrainingStatusAsync(string actionId, string modelName, int version)
        {
            await dbContext.TrainingHistory.UpdateOneAsync(h => h.ModelName == modelName,
                Builders<ModelTrainingHistory>.Update.PullFilter(d => d.ModelVersions, v => v.Version == version));
        }

        protected async override Task UpdateTrainingStatusAsync(string actionId, ModelTrainerContext context, TrainingStatusUpdatingOptions options, int version)
        {
            var modelName = context.ModelName;

            var history = await dbContext.TrainingHistory.AsQueryable()
                .FirstOrDefaultAsync(h => h.ModelName == modelName);

            if (history == null)
            {
                await dbContext.TrainingHistory.InsertOneAsync(new ModelTrainingHistory
                {
                    ModelName = modelName,
                    CurrentVersion = version,
                    ModelVersions = new List<ModelVersion>
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
                    history.ModelVersions = new List<ModelVersion>();

                var tmpVersion = history.ModelVersions.FirstOrDefault(v => v.Version == version);

                if (tmpVersion == null)
                    history.ModelVersions.Add(new ModelVersion
                    {
                        Version = version,
                        TrainedAt = context.TaskRunTime
                    });
                else
                    tmpVersion.TrainedAt = context.TaskRunTime;

                history.CurrentVersion = version;

                await dbContext.TrainingHistory.ReplaceOneAsync(h => h.ModelName == modelName, history);
            }
        }
    }
}

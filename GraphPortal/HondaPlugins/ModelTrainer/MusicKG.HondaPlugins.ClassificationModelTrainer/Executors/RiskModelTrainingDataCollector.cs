using MusicKG.DataManager.Models;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.ModelTrainer.Settings;
using MusicKG.Scheduler.Engine.Exceptions;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using MusicKG.Utility;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace MusicKG.HondaPlugins.ModelTrainer.Executors
{
    public class RiskModelTrainingDataCollector : TrainingDataCollector
    {
        private readonly IHondaMongoDbContext dbContext;
        private new readonly HondaRiskModelTrainingSettings settings;

        public RiskModelTrainingDataCollector(
            HondaRiskModelTrainingSettings settings,
            IHondaMongoDbContext dbContext,
            ILogger<RiskModelTrainingDataCollector> logger) : base(settings, logger)
        {
            this.dbContext = dbContext;
            this.settings = settings;
        }

        protected async override Task<int> CountTrainingDataAsync(string actionId, ModelTrainerContext context, TrainingDataCollectionOptions options)
        {
            return await dbContext.WarningRecord.AsQueryable().CountAsync(r => r.RiskMetrics != null && r.RiskMetrics.UsedForModel);
        }

        protected async override Task<ModelVersion> GetLatestVersionAsync(string modelName)
        {
            var history = await dbContext.TrainingHistory.AsQueryable()
                .FirstOrDefaultAsync(h => h.ModelName == modelName);

            return history?.ModelVersions?.OrderByDescending(v => v.TrainedAt)?.FirstOrDefault();
        }

        protected async override Task<string> SaveTrainingDataAsync(string actionId, 
            ModelTrainerContext context, 
            TrainingDataCollectionOptions options)
        {
            IList<IList<string>> trainingData = null;

            var folder = Path.Combine(settings.TrainingDataFolder, $"{context.LastModelVersion + 1}");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (File.Exists(settings.InitialTrainingFullFileName))
            {
                using (var stream = File.OpenRead(settings.InitialTrainingFullFileName))
                {
                    if (!ExcelHelper.TryReadTable(stream, out trainingData))
                    {
                        throw new JobExecuteException("The initial training data file cannot be parsed.", actionId);
                    }
                }
            }

            var mapping = await dbContext.AIRiskWarningMapping.AsQueryable().ToListAsync();

            var currentData = await dbContext.WarningRecord.AsQueryable().Where(r => r.RiskMetrics != null && r.RiskMetrics.UsedForModel).ToListAsync();

            var currentTrainingData = currentData?.Select(d => d.ToModelTrainingData(mapping))?.ToList();

            (trainingData as List<IList<string>>).AddRange(currentTrainingData);

            foreach (var map in mapping)
            {
                await dbContext.AIRiskWarningMapping.ReplaceOneAsync(a => a.Id == map.Id, map, new ReplaceOptions { IsUpsert = true });
            }

            if (trainingData == null || trainingData.Count == 0)
                throw new JobExecuteException("There is no data to be trained.", actionId);

            File.WriteAllBytes(Path.Combine(folder, settings.TrainingFileName), ExcelHelper.ConvertTableToExcelBytes(trainingData));

            return folder;
        }
    }
}

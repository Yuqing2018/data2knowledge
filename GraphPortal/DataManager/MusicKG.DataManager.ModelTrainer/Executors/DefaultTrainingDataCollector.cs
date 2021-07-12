using MusicKG.DataManager.ModelTrainer.Helpers;
using MusicKG.DataManager.ModelTrainer.Settings;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;
using MusicKG.DataManager.Models.Enums;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Service.Extensions;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace MusicKG.DataManager.ModelTrainer.Executors
{
    /// <summary>
    /// Customerized executor.
    /// </summary>
    public class DefaultTrainingDataCollector : TrainingDataCollector
    {
        private readonly DefaultModelTrainerDbSettings dbSettings;
        private readonly IMongoCollection<BsonDocument> trainingDataCollection;
        private readonly IMongoCollection<ModelTrainingHistory> trainingHistoryCollection;

        public DefaultTrainingDataCollector(ModelTrainingSettings settings, DefaultModelTrainerDbSettings dbSettings, 
            ILogger<DefaultTrainingDataCollector> logger)
            : base(settings, logger)
        {
            this.dbSettings = dbSettings;

            var mongoSettings = MongoClientSettings.FromConnectionString(dbSettings.ConnectionString);

            mongoSettings.MaxConnectionIdleTime = TimeSpan.FromSeconds(int.MaxValue);

            var client = new MongoClient(mongoSettings);

            var database = client.GetDatabase(dbSettings.Database);

            trainingDataCollection = database.GetCollection<BsonDocument>(dbSettings.TrainingDataTableName);
            
            trainingHistoryCollection = database.GetCollection<ModelTrainingHistory>(dbSettings.TrainingHistoryTableName);

            ExecutorType = ModelTrainingExecutors.DefaultTrainingDataCollector.ToString();
        }

        protected async override Task<int> CountTrainingDataAsync(string actionId, 
            ModelTrainerContext context, 
            TrainingDataCollectionOptions options)
        {
            return await trainingDataCollection.AsQueryable().CountAsync();
        }

        protected async override Task<ModelVersion> GetLatestVersionAsync(string modelName)
        {
            var history = await trainingHistoryCollection.AsQueryable()
                .FirstOrDefaultAsync(h => h.ModelName == modelName);
            return history.ModelVersions.OrderByDescending(v => v.TrainedAt).FirstOrDefault();
        }

        protected async override Task<string> SaveTrainingDataAsync(string actionId, 
            ModelTrainerContext context, 
            TrainingDataCollectionOptions options)
        {
            var trainingData = trainingDataCollection.AsQueryable().ToEnumerable();

            return SaveTrainingData(actionId, context, trainingData);
        }

        private string SaveTrainingData(string actionId, ModelTrainerContext context, 
            IEnumerable<BsonDocument> trainingData)
        {
            var folder = Path.Combine(settings.TrainingDataFolder, $"{context.LastModelVersion + 1}");

            if (Directory.Exists(folder))
                Directory.Delete(folder);

            Directory.CreateDirectory(folder);

            var (trainCount, validationCount, testCount) = TrainingDataHelper.AssignTrainingData(context.TrainingDataCount, false);

            var trainData = trainingData.Skip(0).Take(trainCount);

            var validationData = trainingData.Skip(trainCount).Take(validationCount);

            var testData = trainingData.Skip(trainCount + validationCount).Take(testCount);

            var trainFile = Path.Combine(folder, settings.TrainingFiles.TrainDataFileName);

            var validationFile = Path.Combine(folder, settings.TrainingFiles.ValidationDataFile);

            var testFile = Path.Combine(folder, settings.TrainingFiles.TestDataFile);

            logger.LogActionInfo(actionId, $"Saving training data, count: {trainCount}, file: {trainFile}");

            using (var stream = File.CreateText(trainFile))
            {
                var serializer = JsonSerializer.CreateDefault();

                serializer.Serialize(stream, trainData?.Select(t => t.ToJObject()));
            }

            logger.LogActionInfo(actionId, $"Saving training data finished, train data saved to file: {trainFile}");

            logger.LogActionInfo(actionId, $"Saving validation data, count: {validationCount}, file: {validationFile}");

            using (var stream = File.CreateText(validationFile))
            {
                var serializer = JsonSerializer.CreateDefault();

                serializer.Serialize(stream, validationData?.Select(t => t.ToJObject()));
            }

            logger.LogActionInfo(actionId, $"Saving validation data finished, validation data saved to file: {validationFile}");

            logger.LogActionInfo(actionId, $"Saving testing data, count: {testCount}, file: {testFile}");

            using (var stream = File.CreateText(testFile))
            {
                var serializer = JsonSerializer.CreateDefault();

                serializer.Serialize(stream, testData?.Select(t => t.ToJObject()));
            }

            logger.LogActionInfo(actionId, $"Saving testing data finished, testing data saved to file: {testFile}");

            return folder;
        }
    }
}
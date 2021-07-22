using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.ModelTrainer.Data;
using MusicKG.HondaPlugins.ModelTrainer.Extensions;
using MusicKG.HondaPlugins.ModelTrainer.Settings;
using MusicKG.DataManager.ModelTrainer.Executors.Abstractions;
using MusicKG.DataManager.ModelTrainer.Contexts;
using MusicKG.DataManager.ModelTrainer.Options;
using MusicKG.DataManager.ModelTrainer.Helpers;
using MusicKG.DataManager.ModelTrainer.Settings;
using MusicKG.HondaPlugins.DataAccess.DataModels.Business;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using MusicKG.HondaPlugins.DataAccess.Enums;

namespace MusicKG.HondaPlugins.ModelTrainer.Executors
{
    public class ClassificationModelTrainingDataCollector : TrainingDataCollector
    {
        private readonly IHondaMongoDbContext dbContext;
        private readonly MariaRawDataDbSettings rawDataDbSettings;
        private new readonly HondaClassificationModelTrainingSettings settings;

        public ClassificationModelTrainingDataCollector(
            HondaClassificationModelTrainingSettings settings,
            MariaRawDataDbSettings rawDataDbSettings,
            IHondaMongoDbContext dbContext,
            ILogger<ClassificationModelTrainingDataCollector> logger) : base(settings, logger)
        {
            this.settings = settings;
            this.rawDataDbSettings = rawDataDbSettings;
            this.dbContext = dbContext;
            ExecutorType = HondaModelTrainerExecutors.ClassificationModelTrainingDataCollector.ToString();
        }

        protected async override Task<int> CountTrainingDataAsync(string actionId,
            ModelTrainerContext context,
            TrainingDataCollectionOptions options)
        {
            var querable = dbContext.TrainingData.AsQueryable()
                .Where(t => (t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)] != null && t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)] != "" && t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)] != ConstantSettings.UnknownString) ||
                    (t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)] != null && t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)] != "" && t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)] != ConstantSettings.UnknownString));

            return await querable.CountAsync();
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
            var folder = Path.Combine(settings.TrainingDataFolder, $"{context.LastModelVersion + 1}");

            var (partFolder, syndromeFolder) = CreateTrainingFolder(folder);

            var parts = await dbContext.Parts.AsQueryable().ToListAsync();

            var trainingData = GetTrainingData(actionId, parts?.ToDictionary(k => k.No, v => v.Names.FirstOrDefault()));

            var sydromes = dbContext.Syndromes.AsQueryable()
                .Select(s => new IdNameObject
                {
                    MongoId = s.Id,
                    Name = s.Name
                }).ToList();

            var partNames = parts.SelectMany(p => p.Names).ToList()?.ToHashSet();

            SaveTrainingData(actionId, partFolder, settings.PartNameFiles, trainingData?.PartModelTrainingData, true, partNames, sydromes);

            SaveTrainingData(actionId, syndromeFolder, settings.SyndromeFiles, trainingData?.SyndromeModelTrainingData, false, partNames, sydromes);

            SaveSydromesData(actionId, syndromeFolder, settings.SyndromeFiles, sydromes);

            SavePartNameData(actionId, partFolder, settings.PartNameFiles.PartNamesFileName, partNames);

            return folder;
        }

        private (string partFolder, string syndromeFolder) CreateTrainingFolder(string folder)
        {
            var partFolder = Path.Combine(folder, settings.PartNameFiles.PartTrainingDataFolder);
            var syndromeFolder = Path.Combine(folder, settings.SyndromeFiles.SyndromeTrainingDataFolder);

            CreateFolder(folder);
            CreateFolder(partFolder);
            CreateFolder(syndromeFolder);

            return (partFolder, syndromeFolder);
        }

        private void CreateFolder(string folder)
        {
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            Directory.CreateDirectory(folder);
        }

        private HondaModelTrainingData GetTrainingData(string actionId, Dictionary<string, string> parts)
        {
            var partNameQuerable = dbContext.TrainingData.AsQueryable()
                .Where(t => t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)] != null && t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)] != "" && t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)] != ConstantSettings.UnknownString);

            var syndromeQuerable = dbContext.TrainingData.AsQueryable()
                .Where(t => t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)] != null && t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)] != "" && t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)] != ConstantSettings.UnknownString);

            return new HondaModelTrainingData
            {
                PartModelTrainingData = GetPartNameTrainData(actionId, partNameQuerable, parts),
                SyndromeModelTrainingData = GetSyndromeTrainData(actionId, syndromeQuerable)
            };
        }

        private ClassificationModelTrainingData GetPartNameTrainData(string actionId, IMongoQueryable<ModelTrainingData> querable, Dictionary<string, string> parts)
        {
            if (settings.TrainingDataCount > 0)
            {
                querable = querable.OrderByDescending(t => t.Timestamp).Take(settings.TrainingDataCount);
            }

            var dataCount = querable.Count();

            if (dataCount >= settings.TrainingDataCount)
                return GetTrainDataFromMaster(actionId, querable, dataCount);
            else
            {
                var manualData = querable.ToEnumerable();

                var mqiData = GetTrainDataFromRaw(actionId, settings.TrainingDataCount - dataCount, parts);

                var allTrainData = manualData.Concat(mqiData);

                var (trainCount, validationCount, testCount) = TrainingDataHelper.AssignTrainingData(settings.TrainingDataCount, false);

                logger.LogActionInfo(actionId, $"There are {dataCount} training data, {trainCount} for training, {validationCount} for validation, {testCount} for testing.");

                var trainData = allTrainData.Skip(0).Take(trainCount);

                var validationData = allTrainData.Skip(trainCount).Take(validationCount);

                var testData = allTrainData.Skip(trainCount + validationCount).Take(testCount);

                return new ClassificationModelTrainingData
                {
                    TrainData = trainData,
                    ValidationData = validationData,
                    TestData = testData
                };
            }
        }

        private IEnumerable<ModelTrainingData> GetTrainDataFromRaw(string actionId, int takeCount, Dictionary<string, string> parts)
        {
            var connectionString = rawDataDbSettings.ConnectionString;

            var query = $"SELECT MQI_NO,CAR_MODEL,SUBJECT,SYMPTOM_DESC,MAINTENANCE_PROCESS,PART_NO,FAULT_DATE FROM rawmqidata ORDER BY FAULT_DATE DESC LIMIT 0,{takeCount}";

            return ExecuteRawQuery(connectionString, query, parts);
        }

        private IEnumerable<ModelTrainingData> ExecuteRawQuery(string connectionString, string query, Dictionary<string, string> parts)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var com = new MySqlCommand(query, connection))
                {
                    using (var reader = com.ExecuteReader())
                    {
                        var fieldCount = reader.FieldCount;
                        while (reader.Read())
                        {
                            var no = reader.GetString("MQI_NO")?.Trim();

                            var subject = reader.IsDBNull(1) ? "" : reader.GetString("SUBJECT")?.Trim();
                            var carModel = reader.IsDBNull(2) ? "" : reader.GetString("CAR_MODEL")?.Trim();
                            var symtomDesc = reader.IsDBNull(3) ? "" : reader.GetString("SYMPTOM_DESC")?.Trim();
                            var maintenanceProcess = reader.IsDBNull(4) ? "" : reader.GetString("MAINTENANCE_PROCESS")?.Trim();
                         
                            var partNo = reader.IsDBNull(5) ? "" : reader.GetString("PART_NO")?.Trim()?.Substring(0, 5);

                            var faultDate = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime("FAULT_DATE");

                            if (string.IsNullOrWhiteSpace(no) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(carModel) ||
                                string.IsNullOrWhiteSpace(symtomDesc) || string.IsNullOrWhiteSpace(maintenanceProcess) ||
                                string.IsNullOrWhiteSpace(partNo) || faultDate == DateTime.MinValue)
                                continue;

                            if (carModel == "EA6")
                                partNo = $"EA6_{partNo}";

                            if (!parts.ContainsKey(partNo))
                                continue;

                            var partName = parts[partNo];

                            yield return new ModelTrainingData
                            {
                                Id = no,
                                DataSource = DataSource.MQI.ToString(),
                                Timestamp = faultDate,
                                InputFeatures = new Dictionary<string, string>
                                {
                                    { "主题", subject },
                                    { "问诊内容", symtomDesc },
                                    { "诊断结果", maintenanceProcess }
                                },
                                OutputFeatures = new Dictionary<string, string>
                                {
                                    { "Syndrome", "" },
                                    { "PartName", partName }
                                }
                            };
                        }
                    }
                }
            }
        }

        private ClassificationModelTrainingData GetSyndromeTrainData(string actionId, IMongoQueryable<ModelTrainingData> querable)
        {
            if (settings.TrainingDataCount > 0)
            {
                querable = querable.OrderByDescending(t => t.Timestamp).Take(settings.TrainingDataCount);
            }

            var dataCount = querable.Count();

            return GetTrainDataFromMaster(actionId, querable, dataCount);
        }

        private ClassificationModelTrainingData GetTrainDataFromMaster(string actionId, IMongoQueryable<ModelTrainingData> querable, int dataCount)
        {
            var (trainCount, validationCount, testCount) = TrainingDataHelper.AssignTrainingData(dataCount, false);

            logger.LogActionInfo(actionId, $"There are {dataCount} training data, {trainCount} for training, {validationCount} for validation, {testCount} for testing.");

            var trainData = querable.Skip(0).Take(trainCount).ToEnumerable();

            var validationData = querable.Skip(trainCount).Take(validationCount).ToEnumerable();

            var testData = testCount == 0 ? validationData : querable.Skip(trainCount + validationCount).Take(testCount).ToEnumerable();

            return new ClassificationModelTrainingData
            {
                TrainData = trainData,
                ValidationData = validationData,
                TestData = testData
            };
        }

        private void SaveSydromesData(string actionId, string syndromeFolder, SyndromeModelTrainingDataFiles sydromesfiles, IEnumerable<IdNameObject> syndromes)
        {
            logger.LogActionInfo(actionId, $"Saving sydromes data to file: {sydromesfiles.SyndromesFileName}.");

            File.WriteAllLines(Path.Combine(syndromeFolder, sydromesfiles.SyndromesFileName), syndromes?.Select(s => s.Name));

            logger.LogActionInfo(actionId, $"Sydromes data have been saved to file: {sydromesfiles.SyndromesFileName}.");

            logger.LogActionInfo(actionId, $"Saving sydromes Id and name mapping data to file: {sydromesfiles.SyndromeIdMappingFileName}.");

            File.WriteAllText(Path.Combine(syndromeFolder, sydromesfiles.SyndromeIdMappingFileName), JsonConvert.SerializeObject(syndromes));

            logger.LogActionInfo(actionId, $"Sydrome Id and name mapping data have been saved to file: {sydromesfiles.SyndromeIdMappingFileName}.");
        }

        private void SaveTrainingData(string actionId, string folder, ModelTrainingDataFile fileSettings, ClassificationModelTrainingData data, bool isPartName, HashSet<string> parts, List<IdNameObject> syndromes)
        {
            var trainFile = Path.Combine(folder, fileSettings.TrainDataFileName);
            var validationFile = Path.Combine(folder, fileSettings.ValidationDataFile);
            var testFile = Path.Combine(folder, fileSettings.TestDataFile);

            var tasks = new List<Task>
            {
                SaveDataAsync(actionId, FilterTrainingData(actionId, data.TrainData, isPartName, parts, syndromes), trainFile),
                SaveDataAsync(actionId, FilterTrainingData(actionId, data.ValidationData, isPartName, parts, syndromes), validationFile),
                SaveDataAsync(actionId, FilterTrainingData(actionId, data.TestData, isPartName, parts, syndromes), testFile)
            };

            Task.WhenAll(tasks).GetAwaiter().GetResult();
        }

        private IEnumerable<ModelTrainingData> FilterTrainingData(string actionId, IEnumerable<ModelTrainingData> trainingData, bool isPartName, HashSet<string> parts, List<IdNameObject> syndromes)
        {
            return trainingData.Where(t =>
            {
                if (isPartName && !string.IsNullOrWhiteSpace(t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)]) &&
                    t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)] != ConstantSettings.UnknownString)
                {
                    if (!parts.Contains(t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)]))
                    {
                        logger.LogActionWarning(actionId, $"There is part name {t.OutputFeatures[nameof(VehicleFaultDataModel.PartName)]} not in the labeling DB.");
                        return false;
                    }
                    return true;
                }

                if (!isPartName && !string.IsNullOrWhiteSpace(t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)]) &&
                    t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)] != ConstantSettings.UnknownString)
                {
                    if (!syndromes.Any(s => t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)] == s.Name))
                    {
                        logger.LogActionWarning(actionId, $"There is syndrome {t.OutputFeatures[nameof(VehicleFaultDataModel.Syndrome)]} not in the labeling DB.");
                        return false;
                    }
                    return true;
                }

                return false;
            });
        }

        private async Task SaveDataAsync(string actionId, IEnumerable<ModelTrainingData> data, string fileName)
        {
            logger.LogActionInfo(actionId, $"Saving data to file: {fileName}.");

            File.WriteAllLines(fileName, data?.Select(t => t.ToLine()));

            logger.LogActionInfo(actionId, $"Data have been saved to file: {fileName}.");
        }

        private void SavePartNameData(string actionId, string partNameFolder, string partNameFile, IEnumerable<string> partNames)
        {
            logger.LogActionInfo(actionId, $"Saving part names to file: {partNameFile}.");

            File.AppendAllLines(Path.Combine(partNameFolder, partNameFile), partNames);

            logger.LogActionInfo(actionId, $"Part names have been saved to file: {partNameFile}.");
        }
    }
}

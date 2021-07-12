using MusicKG.DataManager.Models;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.Scheduler.Service;
using MusicKG.Scheduler.Engine;
using MusicKG.HondaPlugins.OperationTool.Settings;
using MusicKG.Utility;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    public class HondaModelOperator : IModelOperator
    {
        public const string ClassificationTrainJobName = "classification-model-trainer";
        public const string DecisionTreeTrainJobName = "riskwarning-model-trainer";

        private readonly HondaClassificationModelTrainingSettings classificationSettings;
        private readonly HondaRiskModelTrainingSettings riskSettings;
        private readonly IHondaMongoDbContext context;
        private readonly IJobService jobService;
        private readonly IJobEngine jobEngine;
        private readonly ILogger<HondaModelOperator> logger;

        public HondaModelOperator(
            HondaClassificationModelTrainingSettings classificationSettings,
            HondaRiskModelTrainingSettings riskSettings,
            IHondaMongoDbContext context,
            IJobService jobService,
            IJobEngine jobEngine,
            ILogger<HondaModelOperator> logger)
        {
            this.classificationSettings = classificationSettings;
            this.riskSettings = riskSettings;
            this.context = context;
            this.jobService = jobService;
            this.jobEngine = jobEngine;
            this.logger = logger;
        }

        public async Task<List<ModelTrainingHistory>> ListAsync(string modelName)
        {
            var querable = context.TrainingHistory.AsQueryable();

            if (!string.IsNullOrWhiteSpace(modelName))
                querable = querable.Where(h => h.ModelName == modelName);

            return await querable.ToListAsync();
        }

        public async Task RevertAsync(string modelName, int version)
        {
            var modelNameValue = Enum.Parse<HondaModelNames>(modelName);
            var revertScript = modelNameValue == HondaModelNames.TextClassificationModel ? classificationSettings.RevertModelServingScriptFile : riskSettings.RevertModelServingScriptFile;

            switch (modelNameValue)
            {
                case HondaModelNames.TextClassificationModel:
                    var versionPath = Path.Combine(classificationSettings.ModelServingLocation, "all_part_model", version.ToString());
                    if (!Directory.Exists(versionPath))
                    {
                        logger.LogError($"The model files for version {version} does not exist.");
                        return;
                    }
                    if (ShellHelper.RunShell(classificationSettings.RevertModelServingScriptFile,
                        classificationSettings.ModelServingScriptFolder, TimeSpan.FromHours(1).TotalMilliseconds,
                        error =>
                        {
                            using (var writer = Console.Error)
                            {
                                writer.WriteLine(error);
                            }
                        },
                        info => Console.WriteLine(info),
                        classificationSettings.ModelServingLocation,
                        version.ToString()))
                    {
                        await context.TrainingHistory.UpdateOneAsync(h => h.ModelName == modelName,
                            Builders<ModelTrainingHistory>.Update.Set(x => x.CurrentVersion, version));
                    }
                    break;
                case HondaModelNames.DecisionTreeModel:
                    var riskVersionPath = Path.Combine(riskSettings.TrainedModelFolder, version.ToString());
                    if (!Directory.Exists(riskVersionPath))
                    {
                        logger.LogError($"The model files for version {version} does not exist.");
                        return;
                    }
                    if (ShellHelper.RunShell(riskSettings.RevertModelServingScriptFile,
                        riskSettings.ModelServingScriptFolder, TimeSpan.FromHours(1).TotalMilliseconds,
                        error =>
                        {
                            using (var writer = Console.Error)
                            {
                                writer.WriteLine(error);
                            }
                        },
                        info => Console.WriteLine(info),
                        riskSettings.ModelServingLocation,
                        riskSettings.TrainedModelFolder,
                        version.ToString(),
                        riskSettings.RiskModelInitUrl))
                    {
                        await context.TrainingHistory.UpdateOneAsync(h => h.ModelName == modelName,
                            Builders<ModelTrainingHistory>.Update.Set(x => x.CurrentVersion, version));
                    }
                    break;
            }
        }

        public async Task TrainAsync(string modelName)
        {
            var jobName = modelName == HondaModelNames.TextClassificationModel.ToString() ? ClassificationTrainJobName : DecisionTreeTrainJobName;

            var jobServiceModel = await jobService.GetByNameAsync(jobName);

            await jobEngine.ExecuteAsync(jobServiceModel);
        }
    }
}

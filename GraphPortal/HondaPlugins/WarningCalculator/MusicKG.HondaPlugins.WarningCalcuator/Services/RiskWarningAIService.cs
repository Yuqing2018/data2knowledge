using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.WarningCalculator.Settings;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using MusicKG.HondaPlugins.DataAccess;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.WebApi.ClientWrapper.Extensions;
using MusicKG.Scheduler.Engine.Exceptions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Linq;


namespace MusicKG.HondaPlugins.WarningCalculator.Services
{
    public class RiskWarningAIService : IRiskWarningAIService
    {
        private readonly IHondaMongoDbContext dbContext;
        private readonly RiskWarningModelSettings settings;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<RiskWarningAIService> logger;

        public RiskWarningAIService(IHondaMongoDbContext dbContext, RiskWarningModelSettings settings, IHttpClientFactory httpClientFactory, ILogger<RiskWarningAIService> logger)
        {
            this.dbContext = dbContext;
            this.settings = settings;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task CalculateRiskWarnings(List<WarningRecordDataModel> records, int batchSize)
        {
            if (records == null || records.Count == 0)
                return;

            var httpClient = httpClientFactory.CreateClient();

            var requestCount = (int)Math.Ceiling((decimal)records.Count / batchSize);

            var mapping = await dbContext.AIRiskWarningMapping.AsQueryable().ToListAsync();

            var mappingDic = mapping.ToDictionary(k => k.RiskLevelInModel, v => v.RiskLevel);

            var predictTasks = Enumerable.Range(0, requestCount).Select(index =>
            {
                var recordBatch = records?.Skip(index * batchSize)?.Take(batchSize)?.ToList();

                return PredictBatchAsync(httpClient, recordBatch, mappingDic);
            });

            await Task.WhenAll(predictTasks);
        }

        private async Task PredictBatchAsync(HttpClient httpClient, List<WarningRecordDataModel> records, Dictionary<uint, RiskLevel> mapping)
        {
            var result = await httpClient.PostAsBodyAsync<List<Dictionary<string, object>>, RiskWarningModelResult>(
                settings.ModelRequestUrl, records.Select(record => record.ToModelInput()).ToList());

            if (!result.IsSuccess)
                throw new JobExecuteException($"Caclulate warning task by model failed, details: {result.Message}", WarningCalculatorActions.CalculateWarningAction.ToString());

            if (records.Count != result.PredictLabel.Count)
                throw new JobExecuteException($"Some of records failed when calculated by model.", WarningCalculatorActions.CalculateWarningAction.ToString());

            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].RiskMetrics != null)
                    records[i].RiskMetrics.AIRiskLevel = mapping.ContainsKey(result.PredictLabel[i]) ? mapping[result.PredictLabel[i]] : RiskLevel.一般;
            }
        }
    }
}

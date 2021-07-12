using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.DataManager.Models.Settings;
using MusicKG.Scheduler.DataAccess.Models;
using MusicKG.Scheduler.Service.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace MusicKG.DataManager.Translator.Actions
{
    /// <summary>
    /// Customerized job action.
    /// </summary>
    public class CollectDataAction : JobAction<DataTranslatorContext, DataCollectionOptions, DefaultActionData>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly LabelingServiceSettings labelingServiceSettings;

        public CollectDataAction(IHttpClientFactory httpClientFactory, 
            LabelingServiceSettings labelingServiceSettings,
            Func<string, IActionExecutor> executorFactory, ILogger<CollectDataAction> logger) : base(executorFactory, logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.labelingServiceSettings = labelingServiceSettings;
            defaultExecutorName = DataTranslationDefaultExecutors.DefaultMongoDataCollector.ToString();
            ActionType = DataTranslationActions.DataCollection.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            DataCollectionOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(DataTranslationActions.DataNormalization.ToString(), nextActionData, message);
        }

        public override JobTaskContext CreateContext(DateTime from, DateTime to, JobTaskServiceModel taskDefine)
        {
            var result = new DataTranslatorContext();

            result.From = from;
            result.To = to;
            result.Task = taskDefine;
            result.HttpClient = httpClientFactory.CreateClient();
            result.ServiceUrl = labelingServiceSettings.ServiceUrl;

            result.Initialize();

            return result;
        }
    }
}

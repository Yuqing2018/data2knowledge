using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models.Enums;
using MusicKG.DataManager.Models.Settings;
using MusicKG.Scheduler.Service.Models;
using MusicKG.Scheduler.DataAccess.Models;
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
    public class CollectDocumentAction : JobAction<DataHandlerContext, DocumentCollectionOptions, DefaultActionData>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly LabelingServiceSettings labelingServiceSettings;

        public CollectDocumentAction(IHttpClientFactory httpClientFactory, LabelingServiceSettings labelingServiceSettings,
            Func<string, IActionExecutor> executorFactory, ILogger<CollectDocumentAction> logger) : base(executorFactory, logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.labelingServiceSettings = labelingServiceSettings;
            defaultExecutorName = DataHandlingDefaultExecutors.DefaultDocumentCollector.ToString();
            ActionType = DataHandlingActions.DocumentCollection.ToString();
        }

        protected override JobActionResult GetActionResult(JobActionDataModel action,
            DocumentCollectionOptions options, string message, object nextActionData)
        {
            return JobActionResult.NextAction(DataHandlingActions.DataHandling.ToString(), nextActionData, message);
        }

        public override JobTaskContext CreateContext(DateTime from, DateTime to, JobTaskServiceModel taskDefine)
        {
            var result = new DataHandlerContext();

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

using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Models;
using MusicKG.WebApi.ClientWrapper;
using MusicKG.DataAccess.Enums;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicKG.DataManager.Translator.Executors.Abstractions
{
    public abstract class DocumentCollector<TAnnotationItem> : ActionExecutor<DataHandlerContext, DocumentCollectionOptions, DefaultActionData, DataHandlingActionData> where TAnnotationItem : AnnotationItemModel
    {
        public DocumentCollector(ILogger<DocumentCollector<TAnnotationItem>> logger) : base(logger)
        {
        }

        protected async override Task<DataHandlingActionData> ExecuteInternalAsync(string actionId,
            DataHandlerContext context,
            DocumentCollectionOptions options,
            DefaultActionData data)
        {
            var httpClient = context.HttpClient;

            var token = context.Token;

            IEnumerable<TAnnotationItem> resultData = null;

            var tasks = await TaskWrapper.ListAsync(httpClient, 
                context.ServiceUrl, context.Parameters.WorkspaceId,
                TaskStatusEnum.ConflictResolved, token);

            context.FinishedTaskCount = tasks.Count;

            if (tasks == null || tasks.Count == 0)
            {
                logger.LogActionInfo(actionId, $"There is no task results waiting for handling.");
                return new DataHandlingActionData { Data = null };
            }

            logger.LogActionInfo(actionId, $"There are {tasks.Count} tasks finished and need to be handle.");

            resultData = tasks.SelectMany(task =>
            {
                var documents = TaskWrapper.GetTaskResultsAsync(httpClient,
                    context.ServiceUrl, context.Parameters.WorkspaceId, task,
                    token, TaskDocumentResultTypeEnum.ForGraphMerging).GetAwaiter().GetResult();

                if (documents?.Count == 0)
                    return new List<TAnnotationItem>();

                context.MergedTaskIds.Add(task);

                return documents.SelectMany(document =>
                {
                    var content = DocumentWrapper.DownloadAsync(httpClient,
                        context.ServiceUrl, context.Parameters.WorkspaceId,
                        document, token).GetAwaiter().GetResult();

                    return ConvertDocumentToItems(content);
                });
            });

            return new DataHandlingActionData { Data = resultData?.Cast<AnnotationItemModel>() };
        }

        protected abstract IEnumerable<TAnnotationItem> ConvertDocumentToItems(byte[] documentContent);

        protected async override Task RevertInternalAsync(string actionId, DataHandlerContext context)
        {
            // DO Nothing
        }
    }
}
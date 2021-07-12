using MusicKG.DataManager.Translator.Contexts;
using MusicKG.DataManager.Translator.Data;
using MusicKG.DataManager.Translator.Options;
using MusicKG.DataManager.Models;
using MusicKG.Scheduler.Engine.Action;
using MusicKG.Scheduler.Engine.Extensions;
using MusicKG.WebApi.ClientWrapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace MusicKG.DataManager.Translator.Executors.Abstractions
{
    public abstract class LabelingToolDataConsumer<TAnnotationItem> : ActionExecutor<DataTranslatorContext, DataConsumptionOptions, DataConsumptionActionData, TaskCreationActionData> where TAnnotationItem : AnnotationItemModel
    {
        public LabelingToolDataConsumer(ILogger<LabelingToolDataConsumer<TAnnotationItem>> logger) : base(logger)
        {
        }

        protected abstract IEnumerable<DataPreservationModel> ConvertDataToDocuments(DataTranslatorContext context,
           DataConsumptionOptions options,
           IEnumerable<TAnnotationItem> data);

        protected async override Task<TaskCreationActionData> ExecuteInternalAsync(string actionId,
            DataTranslatorContext context,
            DataConsumptionOptions options,
            DataConsumptionActionData data)
        {
            if (options.Ignore || data == null || data.Data == null || context.DataCount <= 0)
            {
                logger.LogActionInfo("There is no data to be consumed.", actionId);
                return new TaskCreationActionData { Documents = null };
            }

            var result = ConsumeData(actionId, context, options, data.Data?.Cast<TAnnotationItem>());

            return new TaskCreationActionData
            {
                Documents = result
            };
        }

        protected async override Task RevertInternalAsync(string actionId, DataTranslatorContext context)
        {
            if (context.UploadedDocuments?.Count == 0)
            {
                logger.LogActionInfo(actionId, "There is no document has been uploaded, no need to revert.");
                return;
            }

            var httpClient = context.HttpClient;

            context.UploadedDocuments = context.UploadedDocuments.SkipWhile(document =>
            {
                try
                {
                    DocumentWrapper.DeleteAsync(httpClient, context.ServiceUrl,
                        context.Parameters.WorkspaceId, document, context.Token).GetAwaiter().GetResult();
                    return true;
                }
                catch
                {
                    logger.LogActionWarning(actionId, $"Delete document {document} failed.");
                    return false;
                }
            })?.ToList();
        }

        protected virtual IEnumerable<SimpleDocumentModel> ConsumeData(string actionId,
            DataTranslatorContext context,
            DataConsumptionOptions options,
            IEnumerable<TAnnotationItem> data)
        {
            var documents = ConvertDataToDocuments(context, options, data);

            if (documents == null)
            {
                logger.LogActionInfo("There is no document to be uploaded.", actionId);
            }

            return UploadDocuments(actionId, context, options, documents);
        }

        protected virtual IEnumerable<SimpleDocumentModel> UploadDocuments(string actionId,
            DataTranslatorContext context,
            DataConsumptionOptions options,
            IEnumerable<DataPreservationModel> documents)
        {
            var httpClient = context.HttpClient;
            var token = context.Token;

            var uploadResults = documents.Select(document =>
            {
                var updateResult = DocumentWrapper.UploadAsync(httpClient,
                    context.ServiceUrl, token,
                    context.Parameters.WorkspaceId,
                    document.Tags, document.ItemCount, document.FileName,
                    document.ContentType, document.Content).GetAwaiter().GetResult();

                context.UploadedDocuments.Add(updateResult.First());

                return new SimpleDocumentModel
                {
                    Id = updateResult.First(),
                    Name = document.FileName,
                    Tags = document.Tags
                };
            });

            return uploadResults;
        }

        protected virtual byte[] ConvertDataToBytes(DataTranslatorContext context, DataConsumptionOptions options, List<string> tags, IEnumerable<TAnnotationItem> data)
        {
            var documentContent = new DefaultAnnotationDocument<TAnnotationItem>
            {
                DataSource = context.Parameters.DataSourceName,
                Tags = tags,
                Items = data ?? new List<TAnnotationItem>()
            };

            var jsonString = JsonConvert.SerializeObject(documentContent);

            return Encoding.UTF8.GetBytes(jsonString);
        }
    }
}

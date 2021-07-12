using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using MusicKG.DataAccess.Enums;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Settings;

namespace MusicKG.Workflow.Services.Implementations.Processors
{
    /// <summary>
    /// Base class of web api processor used for Knowledge Extraction.
    /// </summary>
    public abstract class KEWebApiProcessor : IProcessorProvider
    {
        protected KEProcessorsSettings settings;
        protected IHttpClientFactory httpClientFactory;
        protected IWorkflowDocumentService workflowDocumentService;
        protected ILogger logger;

        protected string processorName;

        /// <summary>
        /// Constructor of Processor for Knowledge Extraction preprocess.
        /// </summary>
        public KEWebApiProcessor()
        {
        }

        /// <summary>
        /// Initialize processor.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="logger">Logger.</param>
        public virtual void Initialize(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
        {
            this.settings = new KEProcessorsSettings();
            this.settings.ParseFromConfiguration(configuration);

            this.workflowDocumentService = serviceProvider.GetService<IWorkflowDocumentService>();
            this.httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            this.logger = logger;
        }

        /// <summary>
        /// Process action for KE process.
        /// </summary>
        /// <param name="documentId">Document Id.</param>
        /// <param name="stepId">Current step Id.</param>
        /// <param name="status">Status of generated document.</param>
        /// <returns>Process result object.</returns>
        public virtual async Task<DocumentProcessorServiceModel> ProcessAsync(string documentId, string stepId, DocumentStatusEnum status)
        {
            var startTime = DateTime.UtcNow;

            try
            {
                this.logger?.LogInformation($"{this.processorName}: Start processing at {startTime.ToLocalTime()}.");

                this.logger?.LogInformation($"{this.processorName}: Start downloading content for document {documentId}");

                var document = await this.workflowDocumentService.GetDocumentContentAsync(documentId);

                this.logger?.LogInformation($"{this.processorName}: Content was downloaded for document {documentId}.");

                this.logger?.LogInformation($"{this.processorName}: Processing document {documentId} by invoking web api from host {this.settings.ProcessorSettings[this.processorName].RequestUrl}.");

                var respond = await this.InvokeWebServiceAsync(document.Content, document.ContentType);

                this.logger?.LogInformation($"{this.processorName}: Document {documentId} was processed by web api from host {this.settings.ProcessorSettings[this.processorName].RequestUrl}.");

                this.logger?.LogInformation($"{this.processorName}: Start uploading preprocessed document for {documentId}.");

                var newDocumentId = await this.workflowDocumentService.CreateDocumentAsync(new WorkflowDocumentCreateServiceModel
                {
                    ParentId = documentId,
                    Content = Encoding.UTF8.GetBytes(respond.Item1),
                    ContentType = respond.Item2,
                    ParentTags = document.Tags,
                    ParentName = document.Name,
                    ParentWorkflowId = document.WorkflowId.ToString(),
                    ParentWorkspaceId = document.WorkspaceId.ToString(),
                    Status = status,
                    BirthStep = stepId
                });

                this.logger?.LogInformation($"{this.processorName}: Processed document {newDocumentId} was uploaded.");

                this.logger?.LogInformation($"{this.processorName}: Processing succeed at {DateTime.Now}.");

                return new DocumentProcessorServiceModel
                {
                    DocumentId = documentId,
                    Status = DocumentProcessStatusEnum.Succeed,
                    Message = string.Empty,
                    StartTime = startTime,
                    EndTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"{this.processorName}: Process failed at {DateTime.Now}.");

                return new DocumentProcessorServiceModel
                {
                    DocumentId = documentId,
                    Message = ex.Message,
                    Status = DocumentProcessStatusEnum.Failed,
                    StartTime = startTime,
                    EndTime = DateTime.UtcNow
                };
            }
        }

        protected virtual async Task<Tuple<string, string>> InvokeWebServiceAsync(byte[] content, string contentType)
        {
            var httpClient = this.httpClientFactory.CreateClient();

            HttpContent httpContent;

            switch (contentType)
            {
                case "application/json":
                case "text/plain":
                case "text/html":
                    httpContent = new StringContent(Encoding.UTF8.GetString(content));
                    break;
                default:
                    httpContent = new ByteArrayContent(content);
                    break;
            }

            if (httpContent.Headers.ContentType != null)
            {
                httpContent.Headers.ContentType.MediaType = contentType;
            }

            var respond = await httpClient.PostAsync(this.settings.ProcessorSettings[this.processorName].RequestUrl, httpContent);

            respond.EnsureSuccessStatusCode();

            return new Tuple<string, string>(await respond.Content.ReadAsStringAsync(), respond.Content.Headers.ContentType.MediaType);
        }
    }
}

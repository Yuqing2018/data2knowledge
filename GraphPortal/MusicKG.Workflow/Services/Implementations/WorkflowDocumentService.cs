using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MusicKG.DataAccess.Models;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.DataAccess;
using MongoDB.Driver;
using MusicKG.Service.Helpers;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.DataAccess.Enums;
using Microsoft.Extensions.Logging;
using MusicKG.Workflow.Extensions;

namespace MusicKG.Workflow.Services.Implementations
{
    /// <summary>
    /// Workflow document service.
    /// </summary>
    public class WorkflowDocumentService : IWorkflowDocumentService
    {
        private readonly IMusicKGContext context;
        private readonly IStorageService storageService;
        private readonly ILogger<WorkflowDocumentService> logger;

        /// <summary>
        /// Workflow document service constructor.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="storageService">Storage context.</param>
        /// <param name="logger">Logger.</param>
        public WorkflowDocumentService(IMusicKGContext context, 
            IStorageService storageService,
            ILogger<WorkflowDocumentService> logger)
        {
            this.context = context;
            this.storageService = storageService;
            this.logger = logger;
        }

        /// <summary>
        /// Fetch documents by input filter.
        /// </summary>
        /// <param name="filter">Input filter.</param>
        /// <param name="count">Max count to be fetched.</param>
        /// <returns>The list of document Ids.</returns>
        public async Task<List<string>> FetchDocumentsAsync(WorkflowStepInputFilterServiceModel filter, int count)
        {
            try
            {
                var mongoFilter = filter.ToMongoFilter();

                var documentsFound = this.context.Documents.Find(mongoFilter);

                documentsFound = documentsFound.Limit(count);

                var fields = Builders<DocumentDataModel>.Projection.Include(x => x.Id);

                var documents = await documentsFound.Project<DocumentDataModel>(fields).ToListAsync();

                if (documents == null || documents.Count == 0)
                {
                    return null;
                }
                else
                {
                    return documents.Select(x => x.Id.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, "Fetch documents failed.");
                return null;
            }
        }

        /// <summary>
        /// Update documents after processing.
        /// </summary>
        /// <param name="stepId">Processing step id.</param>
        /// <param name="processResult">Processing result.</param>
        /// <returns></returns>
        public async Task UpdateDocumentAsync(string stepId, DocumentProcessorServiceModel processResult)
        {
            try
            {
                var document = await this.context.Documents.Find(x => x.Id == new ObjectId(processResult.DocumentId)).FirstOrDefaultAsync();

                if (document == null)
                {
                    ErrorHelper.ThrowException("Document does not exist.");
                }

                var update = Builders<DocumentDataModel>.Update.Set(x => x.Id, document.Id);

                if (document.NextStep == null)
                {
                    update = update.Set(x => x.NextStep, new DocumentProcessStepDataModel
                    {
                        StepId = new ObjectId(stepId),
                        Status = processResult.Status,
                        Times = 1,
                        Histories = new List<DocumentProcessHistoryDataModel>
                    {
                        new DocumentProcessHistoryDataModel
                        {
                            StartedAt = processResult.StartTime,
                            FinishedAt = processResult.EndTime,
                            Status = processResult.Status,
                            Message = processResult.Message
                        }
                    }
                    });
                }
                else
                {
                    update = update.Set(x => x.NextStep.Status, processResult.Status);
                    update = update.Set(x => x.NextStep.StepId, new ObjectId(stepId));
                    update = update.Set(x => x.NextStep.Times, document.NextStep.Times + 1);
                    if (document.NextStep.Histories == null)
                    {
                        update = update.Set(x => x.NextStep.Histories, new List<DocumentProcessHistoryDataModel>
                    {
                        new DocumentProcessHistoryDataModel
                        {
                            StartedAt = processResult.StartTime,
                            FinishedAt = processResult.EndTime,
                            Status = processResult.Status,
                            Message = processResult.Message
                        }
                    });
                    }
                    else
                    {
                        update = update.Push(x => x.NextStep.Histories, new DocumentProcessHistoryDataModel
                        {
                            StartedAt = processResult.StartTime,
                            FinishedAt = processResult.EndTime,
                            Status = processResult.Status,
                            Message = processResult.Message
                        });
                    }
                }

                document = await this.context.Documents.FindOneAndUpdateAsync<DocumentDataModel>(x => x.Id == new ObjectId(processResult.DocumentId), update, new FindOneAndUpdateOptions<DocumentDataModel>
                {
                    ReturnDocument = ReturnDocument.After
                });
            }
            catch(Exception ex)
            {
                this.logger?.LogError(ex, "Update parent document failed.");
            }
        }

        /// <summary>
        /// Get document metadata and content.
        /// </summary>
        /// <param name="documentId">Document Id.</param>
        /// <returns>Document metadata and content.</returns>
        public async Task<DocumentContentServiceModel> GetDocumentContentAsync(string documentId)
        {
            try
            {
                var document = await this.context.Documents.Find(x => x.Id == new ObjectId(documentId)).FirstOrDefaultAsync();

                var content = await this.storageService.Read(document.ContentMd5);

                return new DocumentContentServiceModel
                {
                    Id = document.Id.ToString(),
                    Name = document.Name,
                    Tags = document.Tags,
                    WorkflowId = document.WorkflowId.ToString(),
                    WorkspaceId = document.WorkspaceId.ToString(),
                    Content = content,
                    ContentType = document.ContentType
                };
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, "Get document content failed.");
                return null;
            }
        }

        /// <summary>
        /// Create document with given content and metadata.
        /// </summary>
        /// <param name="serviceModel">Workflow document create service model.</param>
        /// <returns>Created document Id.</returns>
        public async Task<string> CreateDocumentAsync(WorkflowDocumentCreateServiceModel serviceModel)
        {
            try
            {
                DocumentStoreServiceModel documentStoreServiceModel = new DocumentStoreServiceModel
                {
                    Content = serviceModel.Content,
                    ContentType = serviceModel.ContentType
                };

                var contentMD5 = HashHelper.GetMD5Hash(serviceModel.Content);
                documentStoreServiceModel.ContentMd5 = contentMD5;

                await this.storageService.Create(documentStoreServiceModel);

                var dataModel = new DocumentDataModel
                {
                    Name = serviceModel.ParentName,
                    ContentType = serviceModel.ContentType,
                    WorkspaceId = new ObjectId(serviceModel.ParentWorkspaceId),
                    WorkflowId = new ObjectId(serviceModel.ParentWorkflowId),
                    ContentMd5 = contentMD5,
                    ParentId = new ObjectId(serviceModel.ParentId),
                    Status = serviceModel.Status,
                    Tags = serviceModel.ParentTags,
                    BirthStep = new DocumentProcessStepDataModel
                    {
                        StepId = new ObjectId(serviceModel.BirthStep),
                        Status = DocumentProcessStatusEnum.Succeed
                    },
                    NextStep = null,
                    UploadedAt = DateTime.UtcNow,
                    UploadedBy = ObjectId.Empty,
                };

                await this.context.Documents.InsertOneAsync(dataModel);

                return dataModel.Id.ToString();
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, "Create processed document failed.");
                return null;
            }
        }
    }
}
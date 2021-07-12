using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicKG.Service.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Security.Cryptography;
using MongoDB.Bson;
using System.Net;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MusicKG.Service.Resources;
using MusicKG.Service.SynchronizedWorkflow;

namespace MusicKG.Service.Implementations
{
    /// <summary>
    /// Document service.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IStorageService storage;
        private readonly IMusicKGContext context;
        private readonly IWorkspaceService workspaceService;
        private readonly IWorkflowStepService workflowStepService;
        private readonly ISyncWorkflowStepProcessService syncWorkflowStepProcessService;

        /// <summary>
        /// Document service constructor.
        /// </summary>
        /// <param name="storage">Storage service.</param>
        /// <param name="workspaceService">Workspace service.</param>
        /// <param name="userService">User service.</param>
        /// <param name="context">Database context.</param>
        /// <param name="workflowService">Workflow service.</param>
        public DocumentService(
            IStorageService storage,
            IWorkspaceService workspaceService,
            IMusicKGContext context,
            IWorkflowStepService workflowStepService,
            ISyncWorkflowStepProcessService syncWorkflowStepProcessService)
        {
            this.storage = storage;
            this.context = context;
            this.workspaceService = workspaceService;
            this.workflowStepService = workflowStepService;
            this.syncWorkflowStepProcessService = syncWorkflowStepProcessService;
        }

        /// <summary>
        /// Get document list.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="keyword">Keywork of document tag.</param>
        /// <param name="fromUploadedAt">From uploaded time.</param>
        /// <param name="toUploadedAt">To uploaded time.</param>
        /// <param name="status">Status.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>The list of documents.</returns>
        public async Task<Tuple<long, IEnumerable<DocumentServiceModel>>> GetDocumentsAsync(
            string workspaceId,
            string keyword,
            string tag,
            DateTime? fromUploadedAt,
            DateTime? toUploadedAt,
            IEnumerable<DocumentStatusEnum> status,
            int from,
            int? size)
        {
            var documentFilter = this.context.Documents.AsQueryable().Where(x => !x.IsDeleted && x.WorkspaceId == new ObjectId(workspaceId));

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                documentFilter = documentFilter.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));
            }

            if (fromUploadedAt.HasValue)
            {
                documentFilter = documentFilter.Where(x => x.UploadedAt >= fromUploadedAt);
            }

            if (toUploadedAt.HasValue)
            {
                documentFilter = documentFilter.Where(x => x.UploadedAt <= toUploadedAt);
            }

            if (status != null && status.Count() > 0)
            {
                documentFilter = documentFilter.Where(x => status.Contains(x.Status));
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                documentFilter = documentFilter.Where(x => x.Tags.Any(y => y.ToLower().Contains(tag.ToLower())));
            }

            var uploadStep = await workflowStepService.GetPreannotationWorkflowStepAsync(workspaceId);
            documentFilter = documentFilter.Where(x => x.BirthStep.StepId == new ObjectId(uploadStep.Id));

            var totalCount = await documentFilter.CountAsync();

            documentFilter = documentFilter.Skip(from).Take(size ?? int.MaxValue);

            var documents = await (from d in documentFilter
                                   join u in this.context.Users.AsQueryable() on d.UploadedBy equals u.Id
                                   orderby d.UploadedAt descending
                                   select new DocumentResult
                                   {
                                       ObjectId = d.Id,
                                       Name = d.Name,
                                       Status = d.Status,
                                       Tags = d.Tags,
                                       ContentType = d.ContentType,
                                       UploadedAt = d.UploadedAt,
                                       UploadedByResult = new UserResult
                                       {
                                           ObjectId = u.Id,
                                           Name = u.Name
                                       },
                                       WorkspaceIdResult = d.WorkspaceId
                                   }).ToListAsync();

            return new Tuple<long, IEnumerable<DocumentServiceModel>>(totalCount, documents);
        }

        /// <summary>
        /// Get document.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="documentId">Document Id.</param>
        /// <returns>The document obejct.</returns>
        public async Task<DocumentServiceModel> GetDocumentAsync(string workspaceId, string documentId)
        {
            var documentFilter = this.context.Documents.AsQueryable().Where(x => x.WorkspaceId == new ObjectId(workspaceId)
                && x.Id == new ObjectId(documentId));

            var document = await (from d in documentFilter
                                  join u in this.context.Users.AsQueryable() on d.UploadedBy equals u.Id
                                  select new DocumentResult
                                  {
                                      ObjectId = d.Id,
                                      Name = d.Name,
                                      Status = d.Status,
                                      Tags = d.Tags,
                                      ContentType = d.ContentType,
                                      UploadedAt = d.UploadedAt,
                                      UploadedByResult = new UserResult
                                      {
                                          ObjectId = u.Id,
                                          Name = u.Name
                                      },
                                      WorkspaceIdResult = d.WorkspaceId
                                  }).FirstOrDefaultAsync();

            if (document == null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.DocumentNotExistMessage, HttpStatusCode.BadRequest);
            }

            return document;
        }

        /// <summary>
        /// Get document content.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="documentId">Document Id.</param>
        /// <returns>The document content.</returns>
        public async Task<byte[]> GetDocumentContentAsync(string workspaceId, string documentId)
        {
            var document = await this.GetDocumentDataModel(workspaceId, documentId, null);

            return await this.storage.Read(document.ContentMd5);
        }

        /// <summary>
        /// Upload document.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="serviceModel">Document upload service model.</param>
        /// <returns>Uploaded document service model.</returns>
        public async Task<string> UploadDocumentAsync(string workspaceId, DocumentUploadServiceModel serviceModel)
        {
            var step = await workflowStepService.GetNextWorkflowStepAsync(workspaceId, serviceModel.ParentDocumentId);

            var processedDocument = await syncWorkflowStepProcessService.ProcessAsync(workspaceId, step, serviceModel.Content, serviceModel.ContentType);

            var result = await UploadProcessedDocumentAsync(workspaceId, step, new DocumentUploadServiceModel
            {
                Name = serviceModel.Name,
                ParentDocumentId = serviceModel.ParentDocumentId,
                Tags = serviceModel.Tags,
                UploadedBy = serviceModel.UploadedBy,
                Content = processedDocument.destinationContent,
                ContentType = processedDocument.destinationContentType,
                ItemCount = processedDocument.itemCount == -1 ? serviceModel.ItemCount : processedDocument.itemCount,
            });

            if (step.AutoDoNext)
            {
                return await UploadDocumentAsync(workspaceId, new DocumentUploadServiceModel
                {
                    Name = serviceModel.Name,
                    ParentDocumentId = result,
                    Tags = serviceModel.Tags,
                    UploadedBy = serviceModel.UploadedBy,
                    Content = processedDocument.destinationContent,
                    ContentType = processedDocument.destinationContentType,
                    ItemCount = processedDocument.itemCount == -1 ? serviceModel.ItemCount : processedDocument.itemCount,
                });
            }
            else
            {
                return result;
            }
        }

        private async Task<string> UploadProcessedDocumentAsync(string workspaceId, 
            WorkflowNextStepServiceModel step,
            DocumentUploadServiceModel serviceModel)
        {
            var workspace = await this.workspaceService.GetWorkspaceAsync(workspaceId);

            DocumentDataModel parentDocument = null;
            if (!string.IsNullOrWhiteSpace(serviceModel.ParentDocumentId))
                parentDocument = await GetDocumentDataModel(workspaceId, serviceModel.ParentDocumentId);

            DocumentStoreServiceModel documentStoreServiceModel = new DocumentStoreServiceModel
            {
                Content = serviceModel.Content,
                ContentType = serviceModel.ContentType,
                ContentMd5 = HashHelper.GetMD5Hash(serviceModel.Content)
            };

            await storage.Create(documentStoreServiceModel);

            DocumentDataModel dataModel = new DocumentDataModel
            {
                Name = serviceModel.Name,
                ContentType = serviceModel.ContentType,
                WorkspaceId = new ObjectId(workspaceId),
                WorkflowId = new ObjectId(workspace.Type.WorkflowId),
                ContentMd5 = documentStoreServiceModel.ContentMd5,
                ParentId = parentDocument?.Id,
                Status = step.ResultDocumentStatus,
                Tags = serviceModel.Tags,
                ItemCount = serviceModel.ItemCount,
                BirthStep = new DocumentProcessStepDataModel
                {
                    StepId = new ObjectId(step.Id),
                    Status = DocumentProcessStatusEnum.Succeed
                },
                NextStep = null,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = new ObjectId(serviceModel.UploadedBy),
                IsDeleted = false
            };

            await this.context.Documents.InsertOneAsync(dataModel);

            return dataModel.Id.ToString();
        }

        /// <summary>
        /// Update document.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="documentId">Document Id.</param>
        /// <param name="serviceModel">Document update service model.</param>
        /// <returns></returns>
        public async Task UpdateDocumentAsync(string workspaceId, string documentId, DocumentUpdateServiceModel serviceModel)
        {
            var document = await this.GetDocumentDataModel(workspaceId, documentId);

            var update = Builders<DocumentDataModel>.Update.Set(x => x.Id, document.Id).
                Set(x => x.WorkspaceId, new ObjectId(workspaceId));

            if (serviceModel.IsNameAssigned)
            {
                update = update.Set(x => x.Name, serviceModel.Name);
            }

            if (serviceModel.IsTagsAssigned)
            {
                if (serviceModel.Tags == null || serviceModel.Tags.Count == 0)
                {
                    update = update.Unset(x => x.Tags);
                }
                else
                {
                    update = update.Set(x => x.Tags, serviceModel.Tags);
                }
            }

            if (serviceModel.IsStatusAssigned)
            {
                update = update.Set(x => x.Status, serviceModel.Status);
            }

            document = await this.context.Documents.FindOneAndUpdateAsync<DocumentDataModel>(x => x.Id == new ObjectId(documentId) && x.WorkspaceId == new ObjectId(workspaceId), update,
                new FindOneAndUpdateOptions<DocumentDataModel>
                {
                    ReturnDocument = ReturnDocument.After
                });
        }

        /// <summary>
        /// Delete document.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="documentId">Document Id.</param>
        /// <returns></returns>
        public async Task DeleteDocumentAsync(string workspaceId, string documentId)
        {
            var document = await this.GetDocumentDataModel(workspaceId, documentId);

            var update = Builders<DocumentDataModel>.Update.Set(x => x.IsDeleted, true).Set(x => x.DeletedAt, DateTime.UtcNow);

            await this.context.Documents.FindOneAndUpdateAsync(x => x.Id == new ObjectId(documentId) && x.WorkspaceId == new ObjectId(workspaceId) && x.IsDeleted == false, update);
        }

        private async Task<DocumentDataModel> GetDocumentDataModel(string workspaceId, string documentId, bool? isDeleted = false)
        {
            var builder = Builders<DocumentDataModel>.Filter;

            var filter = builder.Eq(d => d.Id, new ObjectId(documentId))
                & builder.Eq(d => d.WorkspaceId, new ObjectId(workspaceId));

            if (isDeleted.HasValue)
                filter &= builder.Eq(d => d.IsDeleted, isDeleted);

            var document = await context.Documents.Find(filter).FirstOrDefaultAsync();

            if (document == null)
            {
                ErrorHelper.ThrowException(MusicKGMessages.DocumentNotExistMessage, HttpStatusCode.BadRequest);
            }

            return document;
        }
    }

    public class DocumentResult : DocumentServiceModel
    {
        public ObjectId ObjectId
        {
            set
            {
                base.Id = value.ToString();
            }
        }

        public UserResult UploadedByResult
        {
            set
            {
                base.UploadedBy = value;
            }
        }

        public ObjectId WorkspaceIdResult
        {
            set
            {
                base.WorkspaceId = value.ToString();
            }
        }
    }
}

using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Helpers;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Extensions;
using MusicKG.Service.Helpers;
using MusicKG.Service.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text;
namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Document controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;

        /// <summary>
        /// Document controller constructor.
        /// </summary>
        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        /// <summary>
        /// Get document list.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <param name="keyword">Keyword for searching.</param>
        /// <param name="tag">Tag for searching.</param>
        /// <param name="fromUploadedAt">Start upload time.</param>
        /// <param name="toUploadedAt">End upload time.</param>
        /// <param name="statuses">Document status list.</param>
        /// <returns>Document object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DocumentViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly })]
        public async Task<PaginationViewModel<DocumentViewModel>> GetDocuments(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromQuery] int from,
            [FromQuery] int? size,
            [FromQuery] string keyword,
            [FromQuery] string tag,
            [FromQuery] DateTime? fromUploadedAt,
            [FromQuery] DateTime? toUploadedAt,
            [FromQuery] List<DocumentStatusEnum> statuses)
        {
            var (totalCount, documents) = await this.documentService.GetDocumentsAsync(workspaceId,
                keyword, tag, fromUploadedAt, toUploadedAt, statuses, from, size);

            return new PaginationViewModel<DocumentViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = documents.Count(),
                Items = documents.Select(x => this.DocumentServiceModelToViewModel(x))
            };
        }

        /// <summary>
        /// Get document.
        /// </summary>
        /// <param name="workspaceId">Workspace Id.</param>
        /// <param name="documentId">Document Id.</param>
        /// <returns>Document view model.</returns>
        [HttpGet]
        [Route("{documentId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DocumentViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly })]
        public async Task<DocumentViewModel> GetDocument(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string documentId)
        {
            return this.DocumentServiceModelToViewModel(await this.documentService.GetDocumentAsync(workspaceId, documentId));
        }

        /// <summary>
        /// Update document.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="documentId">Document ID.</param>
        /// <param name="bindingModel">Document update binding object.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{documentId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DocumentViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task UpdateDocument(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string documentId,
            [FromBody] [RequiredLocalized] DocumentUpdateBindingModel bindingModel)
        {
            if (bindingModel.IsNameAssigned && string.IsNullOrWhiteSpace(bindingModel.Name))
            {
                ErrorHelper.ThrowException(Service.Resources.MusicKGMessages.DocumentNameEmptyMessage, HttpStatusCode.BadRequest);
            }

            var updateServiceModel = new DocumentUpdateServiceModel
            {
                Name = bindingModel.Name,
                IsNameAssigned = bindingModel.IsNameAssigned,
                Status = bindingModel.Status,
                IsStatusAssigned = bindingModel.IsStatusAssigned,
                Tags = bindingModel.Tags,
                IsTagsAssigned = bindingModel.IsTagsAssigned
            };

            await this.documentService.UpdateDocumentAsync(workspaceId, documentId, updateServiceModel);
        }

        /// <summary>
        /// Delete document.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="documentId">Document ID.</param>
        [HttpDelete]
        [Route("{documentId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteDocument(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string documentId)
        {
            await this.documentService.DeleteDocumentAsync(workspaceId, documentId);
        }

        /// <summary>
        /// Upload document.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="tags">Document tag list.</param>
        /// <param name="documentItems">Items for each document.</param>
        /// <param name="files">Document file list.</param>
        /// <param name="parentDocumentId">Parent document ID.</param>
        /// <returns>Document object list.</returns>
        [HttpPost]
        [Route("Content")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DocumentViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<IEnumerable<string>> UploadDocument(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromForm] [RequiredLocalized] List<string> tags,
            [FromForm][RequiredLocalized] [MinLengthLocalized(1)] List<long> documentItems,
            [FromForm] [RequiredLocalized] [MinLengthLocalized(1)] List<IFormFile> files,
            [FromForm] string parentDocumentId = null)
        {
            List<string> result = new List<string>();
            var currentUser = HttpContextHelper.GetCurrentUser(this.HttpContext);
            Dictionary<string, string> failedDocuments = new Dictionary<string, string>();
            if (documentItems != null && documentItems.Count > 0 && documentItems.Count != files.Count)
            {
                ErrorHelper.ThrowException(Service.Resources.MusicKGMessages.DocumentItemCountWrongMessage, HttpStatusCode.BadRequest);
            }

            if (files.Exists(x => string.IsNullOrWhiteSpace(x.FileName)))
            {
                ErrorHelper.ThrowException(Service.Resources.MusicKGMessages.DocumentNameEmptyMessage, HttpStatusCode.BadRequest);
            }

            string uploadedDocument;
            
            for (int i = 0; i < files.Count; i++)
            {
                try
                {
                    var documentUploadServiceModel = new DocumentUploadServiceModel
                    {
                        Name = System.IO.Path.GetFileNameWithoutExtension(files[i].FileName),
                        Content = files[i].GetContent(),
                        ContentType = files[i].ContentType,
                        Tags = tags ?? new List<string>(),
                        ItemCount = documentItems == null ? 0 : documentItems[i],
                        UploadedBy = currentUser.Item1,
                        ParentDocumentId = parentDocumentId
                    };
                    uploadedDocument = await this.documentService.UploadDocumentAsync(workspaceId, documentUploadServiceModel);
                }
                catch (Exception ex)
                {
                    failedDocuments.Add(files[i].FileName, ex.Message);
                    continue;
                }
                result.Add(uploadedDocument);
            }

            if (failedDocuments.Count > 0)
            {
                var message = new StringBuilder();
                message.AppendLine(string.Format(Service.Resources.MusicKGMessages.DocumentUploadFailedMessage,
                    files.Count, failedDocuments.Count));
                foreach (var failedDocument in failedDocuments)
                {
                    message.AppendLine(string.Format(Service.Resources.MusicKGMessages.DocumentUploadFailedDetailMessage, failedDocument.Key, failedDocument.Value));
                }
                ErrorHelper.ThrowException(message.ToString(), HttpStatusCode.PartialContent);
            }

            return result;
        }

        /// <summary>
        /// Download document.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="documentId">Document ID.</param>
        /// <returns>Document file.</returns>
        [HttpGet]
        [Route("Content/{documentId}")]
        [Produces("text/plain", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [ProducesResponseType(typeof(byte[]), (int)HttpStatusCode.OK)]
        [UserAuthorize]
        public async Task<FileResult> DownloadDocument(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string documentId)
        {
            return File(await this.documentService.GetDocumentContentAsync(workspaceId, documentId), HttpContentTypes.TextPlain);
        }

        private DocumentViewModel DocumentServiceModelToViewModel(DocumentServiceModel serviceModel)
        {
            return new DocumentViewModel
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                Status = serviceModel.Status,
                Tags = serviceModel.Tags,
                UploadedAt = serviceModel.UploadedAt,
                UploadedBy = new UserViewModel
                {
                    Id = serviceModel.UploadedBy.Id,
                    Name = serviceModel.UploadedBy.Name
                },
                WorkspaceId = serviceModel.WorkspaceId
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.WebApi.Filters;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Models;
using MusicKG.WebApi.Helpers;
using MusicKG.Service.Helpers;
using System.Text;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.Service.Constants;

namespace MusicKG.WebApi.Controllers
{
    public partial class TaskController : ControllerBase
    {
        /// <summary>
        /// Get task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="documentId">Document ID.</param>
        /// <param name="resultType">Task document result type.</param>
        /// <returns>Task result object list.</returns>
        [HttpGet]
        [Route("{taskId}/Document/{documentId}/Result")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<TaskResultViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly, UserRoleEnum.Inspector, UserRoleEnum.Acceptor })]
        public async Task<IEnumerable<TaskResultViewModel>> GetResults(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string documentId,
            [FromQuery] TaskDocumentResultTypeEnum? resultType
            )
        {
            (string currentUserId, _, var currentUserRoles) = HttpContextHelper.GetCurrentUser(HttpContext);

            if (string.IsNullOrWhiteSpace(currentUserId))
                ErrorHelper.ThrowException(MusicKGMessages.UserIdWrongMessage, HttpStatusCode.Unauthorized);

            var results = await taskService.GetTaskDocumentResultAsync(
                workspaceId,
                taskId,
                (currentUserRoles.Contains(UserRoleEnum.ReadOnly) ||
                currentUserRoles.Contains(UserRoleEnum.Manager)) ? null : currentUserId,
                documentId,
                resultType);

            return results.Select(r => new TaskResultViewModel
            {
                DocumentId = r.DocumentId,
                ResultType = r.ResultType,
                ResultDocumentId = r.ResultDocumentId,
                TaskDocumentStatus = r.TaskDocumentStatus,
                AnnotatedBy = new UserViewModel
                {
                    Id = r.CreatedBy.Id,
                    Name = r.CreatedBy.Name,
                    Roles = r.CreatedBy.Roles
                },
                AnnotatedAt = r.CreatedAt
            });
        }

        /// <summary>
        /// Create task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="documentId">Document ID.</param>
        /// <param name="bindingModel">Task create binding object.</param>
        [HttpPost]
        [Route("{taskId}/Document/{documentId}/Result")]
        [ProducesResponseType(typeof(TaskResultViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.Inspector })]
        public async Task SaveResult(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string documentId,
            [FromBody] [RequiredLocalized] TaskResultCreateBindingModel bindingModel)
        {
            (string currentUserId, _, _) = HttpContextHelper.GetCurrentUser(HttpContext);

            if (string.IsNullOrWhiteSpace(currentUserId))
                ErrorHelper.ThrowException(MusicKGMessages.UserIdWrongMessage, HttpStatusCode.Unauthorized);

            var resultDocument = await documentService.UploadDocumentAsync(workspaceId, new DocumentUploadServiceModel
            {
                Name = RandomStringHelper.RandomString(10),
                ContentType = HttpContentTypes.ApplicationJson,
                Content = Encoding.UTF8.GetBytes(bindingModel.Result),
                UploadedBy = currentUserId,
                ParentDocumentId = documentId
            });

            await taskService.SaveTaskDocumentResultAsync(workspaceId, taskId, currentUserId, documentId, new TaskDocumentResultSaveServiceModel
            {
                ResultDocumentId = resultDocument,
                ResultType = bindingModel.ResultType,
                Status = bindingModel.Status
            });
        }

        /// <summary>
        /// submit many task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskIdList">Task ID List.</param>
        [HttpPost]
        [Route("Results")]
        [ProducesResponseType(typeof(TaskResultViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.Inspector })]
        public async Task SubmitTasks(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody][RequiredLocalized] List<string> taskIdList)
        {
            (string currentUserId, _, _) = HttpContextHelper.GetCurrentUser(HttpContext);

            if (string.IsNullOrWhiteSpace(currentUserId))
                ErrorHelper.ThrowException(MusicKGMessages.UserIdWrongMessage, HttpStatusCode.Unauthorized);

            await taskService.SaveBatchTaskDocumentResults(workspaceId, currentUserId, taskIdList);
        }

        [HttpGet]
        [Route("{taskId}/Results")]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<List<string>> GetTaskResults(
           [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
           [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
           [FromQuery] TaskDocumentResultTypeEnum? resultType)
        {
            return await taskService.GetTaskResultsAsync(workspaceId, taskId, resultType);
        }

        [HttpPost]
        [Route("{taskId}/Results/Merge")]
        [ProducesResponseType(typeof(TaskResultViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task MergeTaskResults(
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute][RequiredLocalized][StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId)
        {
            await taskService.HandleTaskResultAsync(workspaceId.ToString(), taskId.ToString());
        }
    }
}
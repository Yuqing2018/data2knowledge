using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.WebApi.Filters;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Helpers;
using MusicKG.Service.Helpers;
using MusicKG.Service.Resources;
using MusicKG.Service.Constants;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.WebApi.Contract.Attributes;
using Microsoft.Extensions.Localization;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Task controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public partial class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IDocumentService documentService;
        private readonly IStringLocalizer<OptionController> optionLocalizer;

        /// <summary>
        /// Task controller constructor.
        /// </summary>
        public TaskController(
            ITaskService taskService,
            IDocumentService documentService,
            IStringLocalizer<OptionController> optionLocalizer)
        {
            this.taskService = taskService;
            this.documentService = documentService;
            this.optionLocalizer = optionLocalizer;
        }

        /// <summary>
        /// Get task list.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="statuses">Task statuses.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Task object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [Route("/api/[controller]")]
        [ProducesResponseType(typeof(PaginationViewModel<TaskListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Annotator, UserRoleEnum.ReadOnly, UserRoleEnum.Inspector })]
        public async Task<PaginationViewModel<TaskListItemViewModel>> GetTasks(
            [FromQuery] string keyword,
            [FromQuery] List<TaskStatusEnum> statuses,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            return await GetTasks(null, null, keyword, statuses, from, size);
        }

        /// <summary>
        /// Get task list by workspace.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskTypeId">Task type ID.</param>
        /// <param name="statuses">Task statuses.</param>
        /// <param name="keyword"></param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Task object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<TaskListItemViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly, UserRoleEnum.Inspector })]
        public async Task<PaginationViewModel<TaskListItemViewModel>> GetTasks(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromQuery] string taskTypeId,
            [FromQuery] string keyword,
            [FromQuery] List<TaskStatusEnum> statuses,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            (string currentUserId, _, var currentUserRole) = HttpContextHelper.GetCurrentUser(HttpContext);

            if (string.IsNullOrWhiteSpace(currentUserId))
                ErrorHelper.ThrowException(MusicKGMessages.UserIdWrongMessage, HttpStatusCode.Unauthorized);

            var (totalCount, tasks) = await taskService.GetTasksAsync(
                workspaceId,
                (currentUserRole.Contains(UserRoleEnum.ReadOnly) ||
                currentUserRole.Contains(UserRoleEnum.Manager)) ? null : currentUserId,
                keyword,
                statuses,
                from,
                size,
                taskTypeId);

            return new PaginationViewModel<TaskListItemViewModel>()
            {
                TotalCount = totalCount,
                Count = tasks.Count(),
                From = from,
                Items = tasks.Select(t => new TaskListItemViewModel()
                {
                    Overlap = t.Overlap,
                    ExpectedDueAt = t.ExpectedDueAt,
                    Id = t.Id,
                    Workspace = new WorkspaceViewModel
                    {
                        Id = t.Workspace.Id,
                        Name = t.Workspace.Name,
                        Type = new WorkspaceTypeViewModel
                        {
                            Id = t.Workspace.Type.Id,
                            Name = optionLocalizer == null ? t.Workspace.Type.Name : optionLocalizer[t.Workspace.Type.Name]
                        }
                    },
                    Annotators = t.Annotators.Select(u => new UserViewModel()
                    {
                        Id = u.Annotator.Id,
                        Name = u.Annotator.Name,
                        Roles = u.Annotator.Roles
                    }),
                    Name = t.Name,
                    TaskType = t.TaskType,
                    Status = t.Status,
                    CreatedBy = new UserViewModel
                    {
                        Id = t.CreatedBy.Id,
                        Name = t.CreatedBy.Name
                    }
                })
            };
        }

        /// <summary>
        /// Get task.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <returns>Task object.</returns>
        [HttpGet]
        [Route("{taskId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Annotator, UserRoleEnum.ReadOnly, UserRoleEnum.Inspector })]
        public async Task<TaskViewModel> GetTask(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId)
        {
            var task = await taskService.GetTaskAsync(workspaceId, taskId, isIncludingDocuments: false);

            return new TaskViewModel()
            {
                Id = task.Id,
                Name = task.Name,
                Overlap = task.Overlap,
                ActualDueAt = task.ActualDueAt,
                ExpectedDueAt = task.ExpectedDueAt,
                IsAutoApproved = task.IsAutoApproved,
                IsAutoMerged = task.IsAutoMerged,
                Annotators = task.Annotators.Select(m => new UserViewModel()
                {
                    Id = m.Annotator.Id,
                    Name = m.Annotator.Name,
                    CreatedAt = m.Annotator.CreatedAt,
                    Roles = m.Annotator.Roles,
                    Status = m.Annotator.Status,
                    CreatedBy = m.Annotator.CreatedBy,
                }),
                Workspace = new WorkspaceViewModel
                {
                    Id = task.Workspace.Id,
                    Name = task.Workspace.Name,
                    Type = new WorkspaceTypeViewModel
                    {
                        Id = task.Workspace.Type?.Id
                    }
                },
                Status = task.Status,
                CreatedBy = new UserViewModel
                {
                    Id = task.CreatedBy.Id,
                    Name = task.CreatedBy.Name
                },
                DictionaryIds = task.DictionaryIds,
                TaskType = task.TaskType,
                Inspectors = task.Inspectors?.Select(m => new UserViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList(),
                Acceptors = task.Acceptors?.Select(m => new UserViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList()
            };
        }

        /// <summary>
        /// Create task.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Task create binding object.</param>
        /// <returns>Task ID.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<string> CreateTask(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody] [RequiredLocalized] TaskCreateBindingModel bindingModel)
        {
            if (bindingModel.ExpectedDueAt <= DateTime.UtcNow)
            {
                ErrorHelper.ThrowException(MusicKGMessages.TaskExpectedDueAtWrongMessage, HttpStatusCode.BadRequest);
            }

            var currentUser = HttpContextHelper.GetCurrentUser(this.HttpContext);

            return await taskService.CreateTaskAsync(new TaskCreateServiceModel()
            {
                WorkspaceId = workspaceId,
                Name = bindingModel.Name,
                CreateBy = currentUser.Item1,
                Overlap = bindingModel.Overlap,
                AnnotatorIds = bindingModel.AnnotatorIds,
                DocumentIds = bindingModel.DocumentIds,
                ExpectedDueAt = bindingModel.ExpectedDueAt,
                IsAutoApproved = bindingModel.IsAutoApproved,
                IsAutoMerged = bindingModel.IsAutoMerged,
                DictionaryIds = bindingModel.DictionaryIds,
                TaskType = bindingModel.TaskType,
                InspectorIds = bindingModel.InspectorIds,
                AcceptorIds = bindingModel.AcceptorIds
            });
        }

        /// <summary>
        /// Update task.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="bindingModel">Task update binding object.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{taskId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task UpdateTask(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromBody] [RequiredLocalized] TaskUpdateBindingModel bindingModel)
        {

            if (bindingModel.IsNameAssigned && string.IsNullOrEmpty(bindingModel.Name))
                ErrorHelper.ThrowException(MusicKGMessages.TaskNameEmptyMessage, HttpStatusCode.BadRequest);

            await taskService.UpdateTaskAsync(workspaceId, taskId, new TaskUpdateServiceModel()
            {
                Name = bindingModel.Name,
                IsNameAssigned = bindingModel.IsNameAssigned,
                ExpectedDueAt = bindingModel.ExpectedDueAt
            });
        }

        /// <summary>
        /// Delete task.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        [HttpDelete]
        [Route("{taskId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteTask(
            [FromRoute] [RequiredLocalized] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId)
        {
            await taskService.DeleteTaskAsync(workspaceId, taskId);
        }

        /// <summary>
        /// Approve task.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        [HttpPost]
        [Route("{taskId}/Approve")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Inspector, UserRoleEnum.Acceptor })]
        public async Task ApproveTask(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId)
        {
            await taskService.ApproveTaskAsync(workspaceId, taskId);
        }

        /// <summary>
        /// Reject task.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="annotatorId">Annotator ID.</param>
        [HttpPost]
        [Route("{taskId}/Reject")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Inspector, UserRoleEnum.Acceptor })]
        public async Task RejectTask(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromQuery] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string annotatorId = null)
        {
            await taskService.RejectTaskAsync(workspaceId, taskId, annotatorId);
        }

        /// <summary>
        /// ApproveAll for task.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{taskId}/ApproveAll")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Acceptor })]
        public async Task ApproveAll(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId)
        {
            (string currentUserId, _, _) = HttpContextHelper.GetCurrentUser(HttpContext);

            if (string.IsNullOrWhiteSpace(currentUserId))
                ErrorHelper.ThrowException(MusicKGMessages.UserIdWrongMessage, HttpStatusCode.Unauthorized);

            var task = await taskService.GetTaskAsync(workspaceId, taskId);

            if (task == null)
                ErrorHelper.ThrowException(MusicKGMessages.TaskNotExistMessage, HttpStatusCode.BadRequest);

            var documentIds = task.Annotators.SelectMany(m => m.TaskDocuments.Select(t => t.Document.Id)).Distinct().ToList();

            foreach (var documentId in documentIds)
            {
                var taskDocuments = await taskService.GetTaskDocumentResultAsync(workspaceId, taskId, null, documentId, null);

                if (taskDocuments == null || taskDocuments.Count() == 0)
                    ErrorHelper.ThrowException(MusicKGMessages.TaskAnnotatorOrDocumentNotExistMessage, HttpStatusCode.BadRequest);

                var resultContent = await documentService.GetDocumentContentAsync(workspaceId, taskDocuments.First(t => t.TaskDocumentStatus == TaskDocumentStatusEnum.Annotated).ResultDocumentId);

                var resultDocumentId = await documentService.UploadDocumentAsync(workspaceId, new DocumentUploadServiceModel
                {
                    Name = RandomStringHelper.RandomString(10),
                    ContentType = HttpContentTypes.ApplicationJson,
                    Content = resultContent,
                    UploadedBy = currentUserId,
                    ParentDocumentId = documentId
                });

                await taskService.SaveTaskDocumentResultAsync(workspaceId, taskId, currentUserId, documentId, new TaskDocumentResultSaveServiceModel
                {
                    ResultDocumentId = resultDocumentId,
                    ResultType = TaskDocumentResultTypeEnum.ForModelTraining,
                    Status = TaskDocumentStatusEnum.Annotated
                });
            }
        }
    }
}
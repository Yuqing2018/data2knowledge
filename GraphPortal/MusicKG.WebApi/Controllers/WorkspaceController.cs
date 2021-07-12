using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using MusicKG.Service.Resources;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Helpers;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.BindingModels;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Workspace controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService workspaceService;

        /// <summary>
        /// Workspace controller constructor.
        /// </summary>
        public WorkspaceController(IWorkspaceService workspaceService)
        {
            this.workspaceService = workspaceService;
        }

        /// <summary>
        /// Get workspace list.
        /// </summary>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Workspace object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<WorkspaceViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly })]
        public async Task<PaginationViewModel<WorkspaceViewModel>> GetWorkspaces(
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            (string currentUserId, _, var currentUserRole) = HttpContextHelper.GetCurrentUser(HttpContext);

            if (string.IsNullOrWhiteSpace(currentUserId))
                ErrorHelper.ThrowException(MusicKGMessages.UserIdWrongMessage, HttpStatusCode.Unauthorized);

            var (totalCount, workspaces) = await this.workspaceService.GetWorkspacesAsync(userId: currentUserId, from: from, size: size);

            return new PaginationViewModel<WorkspaceViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = workspaces.Count(),
                Items = workspaces.Select(x => this.WorkspaceServiceModelToViewModel(x))
            };
        }

        /// <summary>
        /// Get workspace.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <returns>Workspace object.</returns>
        [HttpGet]
        [Route("{workspaceId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WorkspaceViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly })]
        public async Task<WorkspaceViewModel> GetWorkspace(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId)
        {
            var workspace = await this.workspaceService.GetWorkspaceAsync(workspaceId);

            return this.WorkspaceServiceModelToViewModel(workspace);
        }

        /// <summary>
        /// Create workspace.
        /// </summary>
        /// <param name="bindingModel">Workspace create binding object.</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WorkspaceViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task CreateWorkspace(
            [FromBody] [RequiredLocalized] WorkspaceCreateBindingModel bindingModel)
        {
            var currentUser = HttpContextHelper.GetCurrentUser(HttpContext);

            await workspaceService.CreateWorkspaceAsync(new WorkspaceCreateServiceModel
            {
                Name = bindingModel.Name,
                IsAutoMerging = bindingModel.IsAutoMerging,
                Description = bindingModel.Description,
                Language = bindingModel.Language,
                Type = bindingModel.Type,
                CreatedBy = currentUser.Item1,
                ReadOnlyUsers = bindingModel.ReadOnlyUserIds
            });
        }

        /// <summary>
        /// Update workspace.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Workspace update binding object.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{workspaceId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WorkspaceViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task UpdateWorkspace(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromBody] [RequiredLocalized] WorkspaceUpdateBindingModel bindingModel)
        {
            if (bindingModel.IsNameAssigned && string.IsNullOrEmpty(bindingModel.Name))
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceNameEmptyMessage, HttpStatusCode.BadRequest);

            await workspaceService.UpdateWorkspaceAsync(workspaceId, new WorkspaceUpdateServiceModel
            {
                Name = bindingModel.Name,
                IsNameAssigned = bindingModel.IsNameAssigned,
                Description = bindingModel.Description,
                IsDescriptionAssigned = bindingModel.IsDescriptionAssigned,
                IsAutoMerging = bindingModel.IsAutoMerging,
                ReadOnlyUserIds = bindingModel.ReadOnlyUserIds
            });
        }

        /// <summary>
        /// Delete workspace.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        [HttpDelete]
        [Route("{workspaceId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteWorkspace(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId)
        {
            await this.workspaceService.DeleteWorkspaceAsync(workspaceId);
        }

        private WorkspaceViewModel WorkspaceServiceModelToViewModel(WorkspaceServiceModel serviceModel)
        {
            return new WorkspaceViewModel
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                Description = serviceModel.Description,
                IsAutoMerging = serviceModel.IsAutoMerging,
                Language = serviceModel.Language,
                Type = new WorkspaceTypeViewModel
                {
                    Id = serviceModel.Type.Id,
                    Name = serviceModel.Type.Name
                },
                CreateBy = new UserViewModel
                {
                    Id = serviceModel.CreatedBy?.Id,
                    Name = serviceModel.CreatedBy?.Name
                },
                ReadOnlyUsers = serviceModel.ReadOnlyUsers?.Select(m => new UserViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList()
            };
        }
    }
}
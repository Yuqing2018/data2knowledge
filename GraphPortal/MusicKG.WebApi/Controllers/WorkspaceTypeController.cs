using System;
using System.ComponentModel.DataAnnotations;
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
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Workspace type controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class WorkspaceTypeController : ControllerBase
    {
        private readonly IWorkspaceTypeService workspaceTypeService;

        /// <summary>
        /// Workspace type controller constructor.
        /// </summary>
        public WorkspaceTypeController(IWorkspaceTypeService workspaceTypeService)
        {
            this.workspaceTypeService = workspaceTypeService;
        }

        /// <summary>
        /// Get workspace type list.
        /// </summary>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Workspace type object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<WorkspaceTypeViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly })]
        public async Task<PaginationViewModel<WorkspaceTypeViewModel>> GetWorkspaceTypes(
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, workspaceTypes) = await this.workspaceTypeService.GetWorkspaceTypesAsync(from, size);

            return new PaginationViewModel<WorkspaceTypeViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = workspaceTypes.Count(),
                Items = workspaceTypes.Select(x => this.WorkspaceTypeServiceModelToViewModel(x))
            };
        }

        /// <summary>
        /// Get workspace type.
        /// </summary>
        /// <param name="workspaceTypeId">Workspace type ID.</param>
        /// <returns>Workspace type object.</returns>
        [HttpGet]
        [Route("{workspaceTypeId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WorkspaceTypeViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.ReadOnly })]
        public async Task<WorkspaceTypeViewModel> GetWorkspaceType(
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceTypeId)
        {
            var workspace = await this.workspaceTypeService.GetWorkspaceTypeAsync(workspaceTypeId);

            return this.WorkspaceTypeServiceModelToViewModel(workspace);
        }

        /// <summary>
        /// Create workspace type.
        /// </summary>
        /// <param name="bindingModel">Workspace type create binding object.</param>
        /// <returns>Workspace type object.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WorkspaceTypeViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<WorkspaceTypeViewModel> CreateWorkspaceType(
            [FromBody] [Required] WorkspaceTypeCreateBindingModel bindingModel)
        {
            if (string.IsNullOrEmpty(bindingModel.Name))
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceTypeNameEmptyMessage, HttpStatusCode.BadRequest);

            var workspaceType = await this.workspaceTypeService.CreateWorkspaceTypeAsync(new WorkspaceTypeCreateServiceModel
            {
                Name = bindingModel.Name,
                Status = bindingModel.Status,
                WorkflowId = bindingModel.WorkflowId
            });

            return this.WorkspaceTypeServiceModelToViewModel(workspaceType);
        }

        /// <summary>
        /// Update workspace type.
        /// </summary>
        /// <param name="workspaceTypeId">Workspace type ID.</param>
        /// <param name="bindingModel">Workspace type update binding object.</param>
        /// <returns>Workspace type object.</returns>
        [HttpPut]
        [Route("{workspaceTypeId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WorkspaceTypeViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<WorkspaceTypeViewModel> UpdateWorkspaceType(
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceTypeId,
            [FromBody] [Required] WorkspaceTypeUpdateBindingModel bindingModel)
        {
            if (bindingModel.IsNameAssigned && string.IsNullOrEmpty(bindingModel.Name))
                ErrorHelper.ThrowException(MusicKGMessages.WorkspaceTypeNameEmptyMessage, HttpStatusCode.BadRequest);

            var workspaceType = await this.workspaceTypeService.UpdateWorkspaceTypeAsync(workspaceTypeId, new WorkspaceTypeUpdateServiceModel
            {
                Name = bindingModel.Name,
                IsNameAssigned = bindingModel.IsNameAssigned,
                Status = bindingModel.Status,
                IsStatusAssigned = bindingModel.IsStatusAssigned
            });

            return this.WorkspaceTypeServiceModelToViewModel(workspaceType);
        }
        
        private WorkspaceTypeViewModel WorkspaceTypeServiceModelToViewModel(WorkspaceTypeServiceModel serviceModel)
        {
            return new WorkspaceTypeViewModel
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                WorkflowName = serviceModel.WorkflowName
            };
        }
    }
}
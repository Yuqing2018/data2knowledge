using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.BindingModels;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Constants;

namespace MusicKG.WebApi.Controllers
{
    public partial class TaskController : ControllerBase
    {
        #region SPO

        /// <summary>
        /// Get task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="documentId">Document ID.</param>
        /// <param name="keyword">Keyword.</param>
        /// <param name="fromCreatedAt">Start creation time.</param>
        /// <param name="toCreatedAt">End creation time.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>SPO object list.</returns>
        [HttpGet]
        [Route("{taskId}/SPO")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<SPOViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<PaginationViewModel<SPOViewModel>> GetSPOs(
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromQuery] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string documentId,
            [FromQuery] string keyword,
            [FromQuery] DateTime? fromCreatedAt,
            [FromQuery] DateTime? toCreatedAt,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            return new PaginationViewModel<SPOViewModel>();
        }

        /// <summary>
        /// Create task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="bindingModel">SPO create binding object.</param>
        /// <returns>SPO object.</returns>
        [HttpPost]
        [Route("{taskId}/SPO")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SPOViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<SPOViewModel> CreateSPO(
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromBody] [Required] SPOCreateBindingModel bindingModel)
        {
            return new SPOViewModel();
        }

        /// <summary>
        /// Create task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="spoId">SPO ID.</param>
        /// <param name="bindingModel">SPO update binding object.</param>
        /// <returns>SPO object.</returns>
        [HttpPut]
        [Route("{taskId}/SPO/{spoId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SPOViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<SPOViewModel> UpdateSPO(
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string spoId,
            [FromBody] [Required] SPOUpdateBindingModel bindingModel)
        {
            return new SPOViewModel();
        }

        /// <summary>
        /// Create task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="spoId">SPO ID.</param>
        [HttpDelete]
        [Route("{taskId}/SPO/{spoId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task DeleteSPO(
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string spoId)
        {

        }

        /// <summary>
        /// Create task document result.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task ID.</param>
        /// <param name="bindingModels">SPO merge binding object list.</param>
        [HttpPost]
        [Route("{taskId}/SPO/Merge")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TaskViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task MergeSPOs(
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [Required] [StringLength(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromBody] [Required] [MinLength(1)] List<SPOMergeBindingModel> bindingModels)
        {

        }

        #endregion
    }
}
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.WebApi.Enums;
using MusicKG.WebApi.Filters;
using MusicKG.DataAccess.Enums;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.BindingModels;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Entity controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class EntityController : ControllerBase
    {
        /// <summary>
        /// Entity controller constructor.
        /// </summary>
        public EntityController()
        {
        }

        /// <summary>
        /// Search entities.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Entity search binding object.</param>
        /// <param name="format">Entity output format.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Entity object list.</returns>
        [HttpPost]
        [Route("Search")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<object>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Guest })]
        public async Task<PaginationViewModel<object>> Search(
            [FromRoute] [Required] string workspaceId,
            [FromBody] [Required] EntitySearchBindingModel bindingModel,
            [FromQuery] int from,
            [FromQuery] int? size,
            [FromQuery] EntityOutputFormatEnum format = EntityOutputFormatEnum.SPO)
        {
            return new PaginationViewModel<object>();
        }

        /// <summary>
        /// Get entity.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="entityId">Entity ID.</param>
        /// <param name="format">Result format [spo|object].</param>
        /// <returns>Entity object.</returns>
        [HttpGet]
        [Route("{entityId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<object>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager, UserRoleEnum.Guest })]
        public async Task<PaginationViewModel<object>> GetEntity(
            [FromRoute] [Required] string workspaceId,
            [FromRoute] [Required] string entityId,
            [FromQuery] EntityOutputFormatEnum format = EntityOutputFormatEnum.SPO)
        {
            return new PaginationViewModel<object>();
        }

        /// <summary>
        /// Match entities.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Entity match binding object.</param>
        /// <param name="format">Entity output format.</param>
        /// <returns>Entity object list.</returns>
        [HttpPost]
        [Route("Match")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<object>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<PaginationViewModel<object>> Match(
            [FromRoute] [Required] string workspaceId,
            [FromBody] [Required] EntityMatchBindingModel bindingModel,
            [FromQuery] EntityOutputFormatEnum format = EntityOutputFormatEnum.SPO)
        {
            return new PaginationViewModel<object>();
        }
    }
}
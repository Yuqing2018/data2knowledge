using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.BindingModels;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Model controller.
    /// </summary>
    [Route("api/Workspace/{workspaceId}/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class ModelController : ControllerBase
    {
        /// <summary>
        /// Model controller constructor.
        /// </summary>
        public ModelController()
        {
        }

        /// <summary>
        /// Get model statistic list.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Model statistic object list.</returns>
        [HttpGet]
        [Route("Statistic")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<ModelStatisticViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<PaginationViewModel<ModelStatisticViewModel>> GetModelStatistics(
            [FromRoute] [Required] string workspaceId,
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            return new PaginationViewModel<ModelStatisticViewModel>();
        }

        /// <summary>
        /// Update document.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="bindingModel">Model update binding object.</param>
        /// <returns>Model object.</returns>
        [HttpPost]
        [Route("Statistic")]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<ModelStatisticViewModel> CreateModelStatistic(
            [FromRoute] [Required] string workspaceId,
            [FromBody] [Required] ModelStatisticCreateBindingModel bindingModel)
        {
            return new ModelStatisticViewModel();
        }
    }
}
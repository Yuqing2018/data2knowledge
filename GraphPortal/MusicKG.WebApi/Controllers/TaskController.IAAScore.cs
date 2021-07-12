using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Contract.ViewModels;
using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.Enums;
using MusicKG.DataAccess.Enums;
using MusicKG.Service.Constants;

namespace MusicKG.WebApi.Controllers
{
    public partial class TaskController : ControllerBase
    {
        /// <summary>
        /// Get task IAA score.
        /// </summary>
        /// <param name="workspaceId">Workspace ID.</param>
        /// <param name="taskId">Task entity ID.</param>
        /// <param name="objectType">IAA score object type.</param>
        /// <param name="viewType">IAA score view type.</param>
        /// <returns>Task IAA score object.</returns>
        [HttpGet]
        [Route("{taskId}/IAAScore")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IAAScoreViewModel), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<IAAScoreViewModel> GetIAAScore(
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string workspaceId,
            [FromRoute] [RequiredLocalized] [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)] string taskId,
            [FromQuery] [RequiredLocalized] IAAScoreObjectTypeEnum objectType,
            [FromQuery] [RequiredLocalized] IAAScoreViewTypeEnum viewType)
        {
            return new IAAScoreViewModel();
        }
    }
}
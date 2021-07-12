using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Service.Models;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Contract.ViewModels;

namespace MusicKG.WebApi.Controllers
{
    /// <summary>
    /// Option controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class OptionController : ControllerBase
    {
        private readonly IOptionService optionService;
        private readonly IStringLocalizer<OptionController> localizer;

        /// <summary>
        /// Option controller constructor.
        /// </summary>
        public OptionController(
            IOptionService optionService,
            IStringLocalizer<OptionController> localizer)
        {
            this.optionService = optionService;
            this.localizer = localizer;
        }

        /// <summary>
        /// Get option list.
        /// </summary>
        /// <param name="type">Option type.</param>
        /// <returns>Option object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<OptionViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize]
        public async Task<IEnumerable<OptionViewModel>> GetOptions([FromQuery] OptionTypeEnum? type)
        {
            var optionServiceModels = await this.optionService.GetOptions(type);
            return optionServiceModels.Select(x => OptionServiceModelToViewModel(x));
        }

        private OptionViewModel OptionServiceModelToViewModel(OptionServiceModel serviceModel)
        {
            return new OptionViewModel
            {
                Type = serviceModel.Type,
                Value = serviceModel.Value,
                DisplayName = localizer[serviceModel.DisplayName]
            };
        }
    }
}

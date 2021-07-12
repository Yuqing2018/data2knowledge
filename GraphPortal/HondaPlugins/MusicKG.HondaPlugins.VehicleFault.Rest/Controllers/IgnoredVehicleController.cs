using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Vehiclefault Controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/ignoredvehicle")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class IgnoredVehicleController : ControllerBase
    {
        private readonly ILogger<IgnoredVehicleController> logger;
        private readonly IIgnoredVehicleService ignoredVehicleService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ignoredVehicleService"></param>
        public IgnoredVehicleController(ILogger<IgnoredVehicleController> logger, IIgnoredVehicleService ignoredVehicleService)
        {
            this.logger = logger;
            this.ignoredVehicleService = ignoredVehicleService;
        }

        /// <summary>
        /// List ignored vehicles.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dictionary<string, List<string>>), (int)HttpStatusCode.OK)]
        public async Task<Dictionary<string, List<string>>> ListAsync([FromQuery] string keyword)
        {
            return await ignoredVehicleService.ListAsync(keyword);
        }

        /// <summary>
        /// Add ignored vehicles.
        /// </summary>
        /// <param name="bindingModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task AddAsync([FromBody] IgnoredVehicleBindingModel bindingModel)
        {
            await ignoredVehicleService.AddAsync(bindingModel.CarModel, bindingModel.CarTypes);
        }

        /// <summary>
        /// Update ignored vehicles.
        /// </summary>
        /// <param name="bindingModel"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task UpdateAsync([FromBody] IgnoredVehicleBindingModel bindingModel)
        {
            await ignoredVehicleService.UpdateAsync(bindingModel.CarModel, bindingModel.CarTypes);
        }

        /// <summary>
        /// Delete ignored vehicles.
        /// </summary>
        /// <param name="carModel"></param>
        /// <returns></returns>
        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task DeleteAsync([Required] [FromQuery] string carModel)
        {
            await ignoredVehicleService.DeleteAsync(carModel);
        }
    }
}

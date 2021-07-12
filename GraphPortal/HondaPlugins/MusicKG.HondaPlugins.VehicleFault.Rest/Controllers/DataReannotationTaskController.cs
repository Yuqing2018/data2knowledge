using MusicKG.HondaPlugins.VehicleFault.Rest.Helpers;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.Services.Models.ReannotationTasks;
using MusicKG.Scheduler.DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Warning Task Controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/reannotationtask")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public partial class DataReannotationTaskController : ControllerBase
    {
        private readonly ILogger<DataReannotationTaskController> logger;
        private readonly IDataReannotationTaskService dataReannotationTaskService;

        /// <summary>
        /// Data reannotation task controller construct function.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataReannotationTaskService"></param>
        public DataReannotationTaskController(ILogger<DataReannotationTaskController> logger,
            IDataReannotationTaskService dataReannotationTaskService)
        {
            this.logger = logger;
            this.dataReannotationTaskService = dataReannotationTaskService;
        }

        /// <summary>
        /// Get data reannotation task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(DataReannotationTaskServiceModel), (int)HttpStatusCode.OK)]
        public async Task<DataReannotationTaskServiceModel> Get([FromRoute] string id)
        {
            var result = await dataReannotationTaskService.GetAsync(id);
            return result;
        }

        /// <summary>
        /// Get page list.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="showAll"></param>
        /// <param name="from">from.</param>
        /// <param name="size">page size.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PaginationViewModel<DataReannotationTaskServiceModel>> GetList(
            string keyword = null,
            TaskExecutionResult? status = null,
            bool showAll = true,
            int from = 0, int? size = null)
        {
            var userName = showAll ? null : HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

            var result = await dataReannotationTaskService.ListAsync(keyword, userName, null, null, null, null, null, null, status);

            var pagedResult = result?.Skip(from)?.Take(size ?? int.MaxValue)?.ToList();

            return new PaginationViewModel<DataReannotationTaskServiceModel>
            {
                TotalCount = result.Count,
                From = from,
                Count = pagedResult?.Count ?? 0,
                Items = pagedResult
            };
        }

        /// <summary>
        /// update a reannotation task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="binding">update binding model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task Update([FromRoute] string id, DataReannotationTaskBindingModel binding)
        {
            var userName = HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

            var serviceModel = new DataReannotationTaskManageServiceModel
            {
                Name = binding.Name,
                Description = binding.Description,
                From = binding.From,
                To = binding.To,
                ManagedBy = userName
            };

            var update = await dataReannotationTaskService.GetAsync(id);
            if (update == null)
            {
                logger.LogError("the reannotation task does not exist!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.ReannotationTaskDoesNotExist, HttpStatusCode.BadRequest);
            }

            await dataReannotationTaskService.UpdateAsync(id, serviceModel);
        }

        /// <summary>
        /// Create a reannnotation task.
        /// </summary>
        /// <param name="binding">Warning Task Binding Model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task Create([FromBody] DataReannotationTaskBindingModel binding)
        {
            if(binding.From > binding.To)
            {
                logger.LogError("Start time must be less than end time!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.StartTimeMustBeLessThanEndTime, HttpStatusCode.BadRequest);
            }

            var userName = HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

            var serviceModel = new DataReannotationTaskManageServiceModel
            {
                Name = binding.Name,
                Description = binding.Description,
                From = binding.From,
                To = binding.To,
                ManagedBy = userName
            };

            await dataReannotationTaskService.CreateAsync(serviceModel);
        }

        /// <summary>
        /// Delete a reannotation task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task Delete([FromRoute] string id)
        {
            await dataReannotationTaskService.DeleteAsync(id);
        }
    }
}

using MusicKG.HondaPlugins.VehicleFault.Rest.Helpers;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.BindingModes;
using MusicKG.HondaPlugins.VehicleFault.Rest.Models.ViewModels;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using MusicKG.HondaPlugins.Services.Models.WarningTasks;
using MusicKG.HondaPlugins.Services.Resources;
using MusicKG.HondaPlugins.Services.Repositories;
using MusicKG.HondaPlugins.DataAccess.DataModels.Warnings;
using MusicKG.HondaPlugins.DataAccess.Enums;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.Scheduler.Engine;
using MusicKG.Scheduler.Service;
using MusicKG.Scheduler.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Controllers
{
    /// <summary>
    /// Warning Task Controller.
    /// </summary>
    [Route("api/honda/v1/vehiclefault/warningtask")]
    [ApiController]
    [UserAuthorize]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public partial class WarningTaskController : ControllerBase
    {
        private readonly ILogger<WarningTaskController> logger;
        private readonly IWarningTaskService warningTaskService;
        private readonly IJobTaskExecutor taskExecutor;
        private readonly IJobService jobService;
        private readonly IJobTaskService taskService;

        /// <summary>
        /// Warning task controller construct function.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="warningTaskService"></param>
        /// <param name="jobService"></param>
        /// <param name="taskService"></param>
        /// <param name="taskExecutor"></param>
        public WarningTaskController(ILogger<WarningTaskController> logger,
            IWarningTaskService warningTaskService,
            IJobService jobService,
            IJobTaskService taskService,
            IJobTaskExecutor taskExecutor)
        {
            this.logger = logger;
            this.taskExecutor = taskExecutor;
            this.jobService = jobService;
            this.taskService = taskService;
            this.warningTaskService = warningTaskService;
        }

        /// <summary>
        /// Get warning task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(WarningTaskServiceModel), (int)HttpStatusCode.OK)]
        public async Task<WarningTaskServiceModel> Get([FromRoute] string id)
        {
            var result = await warningTaskService.GetAsync(id);
            return result;
        }

        /// <summary>
        /// 获取所有预警任务Id,名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("options")]
        [ProducesResponseType(typeof(List<WarningTaskIdAndNameModel>), (int)HttpStatusCode.OK)]
        public async Task<List<WarningTaskIdAndNameModel>> GetTaskIdAndNames()
        {
            var result = await warningTaskService.GetTaskIdAndNameList();
            return result;
        }

        /// <summary>
        /// Get page list.
        /// </summary>
        /// <param name="warningUnit">warning unit.</param>
        /// <param name="warningStatus">warning status.</param>
        /// <param name="carModels"></param>
        /// <param name="carTypes"></param>
        /// <param name="modelYears"></param>
        /// <param name="createdBy">user who create the task.</param>
        /// <param name="from">from.</param>
        /// <param name="size">page size.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PaginationViewModel<WarningTaskListViewModel>> GetList(
            [FromQuery] WarningUnit? warningUnit,
            [FromQuery] WarningTaskStatus? warningStatus, 
            [FromQuery] List<string> carModels,
            [FromQuery] List<string> carTypes,
            [FromQuery] List<string> modelYears,
            [FromQuery] string createdBy,
            int from, int? size)
        {
            var (totalCount, datas) = await warningTaskService.GetWarningTasksAsync(warningUnit, warningStatus, createdBy, carModels, carTypes, modelYears, from, size);
            
            return new PaginationViewModel<WarningTaskListViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = datas.Count(),
                Items = datas.Select(u => ServiceModelToListViewModel(u))
            };
        }

        /// <summary>
        /// update a warning task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="binding">update binding model.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task Update([FromRoute] string id, WarningTaskBindingModel binding)
        {
            var update = await warningTaskService.GetAsync(id);
            if (update == null)
            {
                logger.LogError("the warning task does not exist!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.WarningTaskDoesNotExist, HttpStatusCode.BadRequest);
            }

            var isExist = await warningTaskService.IsExist(id, binding.Name);
            if (isExist)
            {
                logger.LogError("the warning task name has been exist!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.WarningTaskNameAlreadyExists, HttpStatusCode.BadRequest);
            }

            var updateModel = new WarningTaskDataModel()
            {
                Name = binding.Name,
                WarningUnit = binding.WarningUnit,
                WarningStatus = update.WarningStatus,
                CarModels = binding.CarModel,
                CarTypes = binding.CarTypes,
                YearModels = binding.YearModels,
                WarningIndex = binding.WarningIndex == null
                    ? update.WarningIndex.Select(x => new WarningIndexDataModel()
                    {
                        Id = new ObjectId(x.Id),
                        IndexName = x.IndexName,
                        Value = x.Value,
                        Unit = x.Unit,
                        WarningType = x.WarningType
                    }).ToList() : binding.WarningIndex?.Select(x => new WarningIndexDataModel()
                    {
                        Id = new ObjectId(x.Id),
                        IndexName = x.IndexName,
                        Value = x.Value,
                        Unit = x.Unit,
                        WarningType = x.WarningType
                    }).ToList(),
                CreateTime = update.CreateTime,
                CreateBy = update.CreateBy,
            };
            await warningTaskService.UpdateAsync(id, updateModel);
        }
        /// <summary>
        /// stop warning task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/termination")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<bool> Stop([FromRoute] string id)
        {
            await VaidateExist(id);
            var result = await warningTaskService.StopAsync(id);
            return result;
        }

        /// <summary>
        /// Create a warning task.
        /// </summary>
        /// <param name="binding">Warning Task Binding Model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task Create([FromBody] WarningTaskBindingModel binding)
        {
            var isExist = await warningTaskService.IsExist(null, binding.Name);
            if (isExist)
            {
                logger.LogError("the warning task name has been exist!");
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.WarningTaskNameAlreadyExists, HttpStatusCode.BadRequest);
            }
            var userName = this.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

            var createModel = new WarningTaskDataModel()
            {
                Name = binding.Name,
                WarningUnit = binding.WarningUnit,
                WarningStatus = WarningTaskStatus.预警中,
                CarModels = binding.CarModel,
                CarTypes = binding.CarTypes,
                YearModels = binding.YearModels,
                WarningIndex = binding.WarningIndex?.Select(x=> new WarningIndexDataModel()
                {
                    Id = new ObjectId(x.Id),
                    WarningType = x.WarningType,
                    IndexName = x.IndexName,
                    Value = x.Value,
                    Unit = x.Unit
                }).ToList(),
                CreateBy = userName
            };

            await warningTaskService.SaveAsync(createModel);
        }

        /// <summary>
        /// Delete a warning task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task Delete([FromRoute] string id)
        {
            await warningTaskService.DeleteAsync(id);
        }

        /// <summary>
        /// Execute a warning task manully.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/execute")]
        [ProducesResponseType(typeof(JobTaskStatus), (int)HttpStatusCode.OK)]
        public async Task<JobTaskStatus> Execute([FromRoute] string id)
        {
            var job = await jobService.GetAsync(ConstantSettings.WarningCalculationJobId);
            var task = await taskService.GetAsync(job.Id, id);

            if (task == null || task.IsDeleted)
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.WarningTaskDoesNotExist, HttpStatusCode.NotFound);

            await taskExecutor.ExecuteAsync(job, task);

            return await taskService.GetStatusAsync(ConstantSettings.WarningCalculationJobId, id);
        }

        private WarningTaskViewModel ServiceModelToViewModel(WarningTaskServiceModel data)
        {
            return new WarningTaskViewModel()
            {
                Id = data.Id.ToString(),
                Name = data.Name,
                WarningStatus = data.WarningStatus,
                WarningUnit = data.WarningUnit,
                CarModels = data.CarModels,
                CarTypes = data.CarTypes,
                YearModels = data.YearModels,
                WarningIndexSetting = data.WarningIndex.Select(x=>new WarningIndexViewModel()
                {
                    Id = x.Id.ToString(),
                    IndexName = x.IndexName,
                    Value = x.Value
                }).ToList(),
                CreateBy = data.CreateBy,
                CreateTime = data.CreateTime
            };
        }

        private WarningTaskListViewModel ServiceModelToListViewModel(WarningTaskServiceModel data)
        {
            return new WarningTaskListViewModel()
            {
                Id = data.Id,
                Name = data.Name,
                WarningStatus = data.WarningStatus,
                WarningUnit = data.WarningUnit,
                WarningType = data.WarningType,
                CarModels = data.CarModels,
                CarTypes = data.CarTypes,
                YearModels = data.YearModels,
                CreateBy = data.CreateBy,
                CreateTime = data.CreateTime
            };
        }

        private async Task VaidateExist(string id)
        {
            var update = await warningTaskService.GetAsync(id);
            if (update == null)
            {
                logger.LogError(MusicKGHondaPluginsMessage.WarningTaskDoesNotExist);
                ErrorHelper.ThrowException(MusicKGHondaPluginsMessage.WarningTaskDoesNotExist, HttpStatusCode.BadRequest);
            }
        }
    }
}

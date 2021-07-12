using MusicKG.Scheduler.Rest.Models.ViewModels;
using MusicKG.Scheduler.Rest.Models.BindingModels;
using MusicKG.Scheduler.Service;
using MusicKG.Scheduler.Service.Models;
using MusicKG.Scheduler.DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System;


namespace MusicKG.Scheduler.Rest.Controllers
{
    [Route("api/scheduler/v1/job/{jobId}/task")]
    [ApiController]
    [ProducesResponseType(typeof(SchedulerErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SchedulerErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class JobTaskController : ControllerBase
    {
        private readonly IJobTaskService jobTaskService;

        public JobTaskController(IJobTaskService jobTaskService)
        {
            this.jobTaskService = jobTaskService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JobTaskListViewModel), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<JobTaskListViewModel>> ListAsync(
            [FromRoute] string jobId,
            [FromQuery] string keyword = null,
            [FromQuery] TaskExecutionResult? status = null,
            [FromQuery] string createdBy = null,
            [FromQuery] bool? isRunOnce = null,
            [FromQuery] DateTime? createdBefore = null,
            [FromQuery] DateTime? createdAfter = null,
            [FromQuery] DateTime? succeedBefore = null,
            [FromQuery] DateTime? succeedAfter = null,
            [FromQuery] DateTime? runBefore = null,
            [FromQuery] DateTime? runAfter = null,
            [FromQuery] bool ignoreDeleted = true)
        {
            var serviceModel = await jobTaskService.ListAsync(jobId, keyword, status, createdBy, isRunOnce, succeedBefore, succeedAfter, createdBefore, createdAfter, runBefore, runAfter, ignoreDeleted);

            return serviceModel?.Select(t => new JobTaskListViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Executor = t.Executor,
                IsRunOnce = t.IsRunOnce,
                LastStatus = t.LastStatus,
                ManagedAt = t.ManagedAt,
                Job = new JobViewModel
                {
                    Id = t.Job.Id,
                    Name = t.Job.Name
                },
                MerchantCreationTime = t.MerchantCreationTime,
                ManagedBy = t.ManagedBy,
                LastSucceedAt = t.LastSucceedAt,
                LastRunAt = t.LastRunAt
            });
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dictionary<string, object>), (int)HttpStatusCode.OK)]
        public async Task<JObject> DetailsAsync([FromRoute] string jobId, [FromRoute] string id)
        {
            return await jobTaskService.GetDetailsAsync(jobId, id);
        }

        [HttpGet("{id}/histories")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JobTaskHistoryViewModel), (int)HttpStatusCode.OK)]
        public async Task<List<JobTaskHistoryViewModel>> GetHistoriesAsync([FromRoute] string jobId, [FromRoute] string id)
        {
            var serviceModel = await jobTaskService.GetHistoryAsync(jobId, id);

            return serviceModel?.Select(t => new JobTaskHistoryViewModel
            {
                RunAt = t.RunAt,
                DeadAction = t.DeadAction,
                IsSucceed = t.IsSucceed,
                Message = t.Message
            })?.ToList();
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task CreateAsync([FromRoute] string jobId, [FromBody] JobTaskBindingModel bindingModel)
        {
            await jobTaskService.CreateAsync(jobId, new JobTaskManagementServiceModel
            {
                Name = bindingModel.Name,
                Description = bindingModel.Description,
                ExecutorDomain = bindingModel.ExecutorDomain,
                ExecutorName = bindingModel.ExecutorName,
                ExecutorPassword = bindingModel.ExecutorPassword,
                IsRunOnce = bindingModel.IsRunOnce,
                MerchantCreationTime = bindingModel.MerchantCreationTime,
                ManagedBy = bindingModel.ManagedBy,
                TaskDefine = bindingModel.TaskDefine
            });
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task UpdateAsync([FromRoute] string jobId, [FromRoute] string id,
            [FromBody] JobTaskBindingModel bindingModel)
        {
            await jobTaskService.UpdateAsync(jobId, id, new JobTaskManagementServiceModel
            {
                Name = bindingModel.Name,
                Description = bindingModel.Description,
                ExecutorDomain = bindingModel.ExecutorDomain,
                ExecutorName = bindingModel.ExecutorName,
                ExecutorPassword = bindingModel.ExecutorPassword,
                IsRunOnce = bindingModel.IsRunOnce,
                MerchantCreationTime = bindingModel.MerchantCreationTime,
                ManagedBy = bindingModel.ManagedBy,
                TaskDefine = bindingModel.TaskDefine
            });
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task DeleteAsync([FromRoute] string jobId, [FromRoute] string id)
        {
            await jobTaskService.DeleteAsync(jobId, id);
        }
    }
}

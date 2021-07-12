using MusicKG.Scheduler.Rest.Models.ViewModels;
using MusicKG.Scheduler.Rest.Models.BindingModels;
using MusicKG.Scheduler.Service.Models;
using MusicKG.Scheduler.Service.Extensions;
using MusicKG.Scheduler.Service;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Scheduler.Rest.Controllers
{
    [Route("api/scheduler/v1/job")]
    [ApiController]
    [ProducesResponseType(typeof(SchedulerErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SchedulerErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class JobController : ControllerBase
    {
        private readonly IJobService jobService;

        public JobController(IJobService jobService)
        {
            this.jobService = jobService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JobViewModel), (int)HttpStatusCode.OK)]
        public async Task<IEnumerable<JobViewModel>> ListAsync([FromQuery] List<string> jobTypes = null)
        {
            var serviceModel = await jobService.ListAsync(jobTypes);

            return serviceModel?.Select(t => new JobViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Schedule = t.Schedule,
                LastRunAt = t.LastRunAt
            });
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task CreateAsync([FromBody] JobBindingModel bindingModel)
        {
            var serviceModel = new JobManagementServiceModel
            {
                Name = bindingModel.Name,
                Description = bindingModel.Description,
                Schedule = bindingModel.Schedule,
                Actions = bindingModel.Actions?.Select(a => new JobActionServiceModel
                {
                    ActionId = a.ActionId,
                    Description = a.Description,
                    IsDefault = a.IsDefault,
                    Options = a.Options.ToBsonDocument()
                })?.ToList()
            };

            await jobService.CreateAsync(serviceModel);
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task UpdateAsync([FromRoute] string id, [FromBody] JobBindingModel bindingModel)
        {
            var serviceModel = new JobManagementServiceModel
            {
                Name = bindingModel.Name,
                Description = bindingModel.Description,
                Schedule = bindingModel.Schedule,
                Actions = bindingModel.Actions?.Select(a => new JobActionServiceModel
                {
                    ActionId = a.ActionId,
                    Description = a.Description,
                    IsDefault = a.IsDefault,
                    Options = a.Options.ToBsonDocument()
                })?.ToList()
            };

            await jobService.UpdateAsync(id, serviceModel);
        }
    }
}

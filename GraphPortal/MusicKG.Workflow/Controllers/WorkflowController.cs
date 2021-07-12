using Microsoft.AspNetCore.Mvc;
using MusicKG.DataAccess.Enums;
using MusicKG.Workflow.Filters;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Models.ViewModels;
using MusicKG.Workflow.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Controllers
{
    /// <summary>
    /// Workflow controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.InternalServerError)]
    public class WorkflowController : ControllerBase
    {
        private IWorkflowService workflowService;

        /// <summary>
        /// Workflow controller constructor.
        /// </summary>
        /// <param name="workflowService">Workflow service.</param>
        public WorkflowController(IWorkflowService workflowService)
        {
            this.workflowService = workflowService;
        }

        /// <summary>
        /// Get workflow list.
        /// </summary>
        /// <param name="from">Pagination start index.</param>
        /// <param name="size">Pagination size.</param>
        /// <returns>Workflow object list.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PaginationViewModel<WorkflowViewModel>), (int)HttpStatusCode.OK)]
        [UserAuthorize(Roles = new UserRoleEnum[] { UserRoleEnum.Manager })]
        public async Task<PaginationViewModel<WorkflowViewModel>> GetWorkflows(
            [FromQuery] int from,
            [FromQuery] int? size)
        {
            var (totalCount, workflows) = await this.workflowService.GetWorkflowsAsync(from, size);

            return new PaginationViewModel<WorkflowViewModel>
            {
                TotalCount = totalCount,
                From = from,
                Count = workflows.Count(),
                Items = workflows.Select(x => this.WorkflowServiceModelToViewModel(x))
            };
        }

        private WorkflowViewModel WorkflowServiceModelToViewModel(WorkflowServiceModel serviceModel)
        {
            return new WorkflowViewModel
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name
            };
        }
    }
}

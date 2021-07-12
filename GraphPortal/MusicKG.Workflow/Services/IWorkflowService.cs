using MusicKG.Workflow.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services
{
    public interface IWorkflowService
    {
        Task<WorkflowServiceModel> GetWorkflowAsync(string id);

        Task<Tuple<long, IEnumerable<WorkflowServiceModel>>> GetWorkflowsAsync(int from, int? size);

        Task<WorkflowServiceModel> CreateWorkflowAsync(WorkflowServiceModel workflow);
    }
}

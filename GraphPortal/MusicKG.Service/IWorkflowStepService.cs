using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service
{
    public interface IWorkflowStepService
    {
        Task<WorkflowNextStepServiceModel> GetNextWorkflowStepAsync(string workspaceId, string parentDocumentId);

        Task<WorkflowNextStepServiceModel> GetPreannotationWorkflowStepAsync(string workspaceId);
    }
}

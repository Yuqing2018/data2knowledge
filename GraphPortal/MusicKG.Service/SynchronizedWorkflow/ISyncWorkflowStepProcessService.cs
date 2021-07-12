using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service.SynchronizedWorkflow
{
    public interface ISyncWorkflowStepProcessService
    {
        Task<(byte[] destinationContent, string destinationContentType, long itemCount)> ProcessAsync(
            string workspaceId,
            WorkflowNextStepServiceModel step,
            byte[] sourceContent,
            string sourceContentType);
    }
}

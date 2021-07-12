using Microsoft.Extensions.Logging;
using MusicKG.DataAccess.Models;
using MusicKG.Service.Helpers;
using MusicKG.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service.SynchronizedWorkflow.Implementations
{
    public class SyncWorkflowStepProcessService : ISyncWorkflowStepProcessService
    {
        private readonly ISyncProcessorFactory syncProcessorFactory;
        private readonly ILogger<SyncWorkflowStepProcessService> logger;

        public SyncWorkflowStepProcessService(ISyncProcessorFactory syncProcessorFactory,
            ILogger<SyncWorkflowStepProcessService> logger)
        {
            this.syncProcessorFactory = syncProcessorFactory;
            this.logger = logger;
        }

        public async Task<(byte[] destinationContent, string destinationContentType, long itemCount)> ProcessAsync(
            string workspaceId,
            WorkflowNextStepServiceModel step,
            byte[] sourceContent,
            string sourceContentType)
        {
            if (string.IsNullOrWhiteSpace(step.AssemblyName) || string.IsNullOrWhiteSpace(step.ClassName))
            {
                return (sourceContent, sourceContentType, -1);
            }
            else
            {
                var processor = syncProcessorFactory.CreateProcessor(step.AssemblyName, step.ClassName);

                return await processor.ProcessAsync(workspaceId, sourceContent, sourceContentType);
            }
        }
    }
}

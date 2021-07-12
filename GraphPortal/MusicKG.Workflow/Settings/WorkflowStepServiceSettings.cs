using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Workflow.Settings
{
    public class WorkflowStepServiceSettings
    {
        private const int DefaultScanRate = 60000;
        private const int DefaultProcessorCount = 5;
        private const int DefaultTimeout = 120000;

        public int ScanRate { get; set; }

        public int ProcessorCount { get; set; }

        public TimeSpan Timeout { get; set; }

        public void ParseFromConfiguration(IConfiguration configuration)
        {
            if (int.TryParse(configuration.GetSection("WorkflowStepProcessor:ScanRate")?.Value, out int scanRate))
            {
                this.ScanRate = scanRate;
            }
            else
            {
                this.ScanRate = WorkflowStepServiceSettings.DefaultScanRate;
            }
            if (int.TryParse(configuration.GetSection("WorkflowStepProcessor:ProcessorCount")?.Value, out int processorCount))
            {
                this.ProcessorCount = processorCount;
            }
            else
            {
                this.ProcessorCount = WorkflowStepServiceSettings.DefaultProcessorCount;
            }
            if (int.TryParse(configuration.GetSection("WorkflowStepProcessor:Timeout")?.Value, out int timeout))
            {
                this.Timeout = TimeSpan.FromMilliseconds(timeout);
            }
            else
            {
                this.Timeout = TimeSpan.FromMilliseconds(WorkflowStepServiceSettings.DefaultTimeout);
            }
        }
    }
}

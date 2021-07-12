using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Settings
{
    public class SupportedWorkflowSettings
    {
        public List<WorkflowSettings> SupportedWorkflows { get; private set; }

        public SupportedWorkflowSettings()
        {
            this.SupportedWorkflows = new List<WorkflowSettings>();
        }

        public void ParseFromParseFromConfiguration(IConfiguration configuration)
        {
            this.SupportedWorkflows = configuration.GetSection("SupportedWorkflows")?.GetChildren()?.Select(x =>
            {
                var result = new WorkflowSettings();
                result.ParseFromParseFromConfiguration(x);
                return result;
            })?.ToList();
        }
    }
}

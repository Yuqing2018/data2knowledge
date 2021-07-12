using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Settings
{
    public class WorkflowSettings
    {
        public string WorkflowId { get; set; }

        public List<string> SupportedStepIds { get; set; }

        public void ParseFromParseFromConfiguration(IConfigurationSection configurationSection)
        {
            this.WorkflowId = configurationSection.GetValue<string>("WorkflowId");
            this.SupportedStepIds = configurationSection.GetSection("SupportedSteps")?.GetChildren()?.Select(x => x.Value)?.ToList();
        }
    }
}

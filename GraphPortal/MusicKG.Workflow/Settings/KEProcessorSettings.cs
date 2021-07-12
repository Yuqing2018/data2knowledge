using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Settings
{
    public class KEProcessorSettings
    {
        public string ProcessorName { get; set; }

        public string RequestUrl { get; set; }

        public void ParseFromConfiguration(IConfigurationSection configurationSection)
        {
            this.ProcessorName = configurationSection.GetSection("ProcessorName")?.Value;
            this.RequestUrl = configurationSection.GetSection("RequestUrl")?.Value;
        }
    }
}

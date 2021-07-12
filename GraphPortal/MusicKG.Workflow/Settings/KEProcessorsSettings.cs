using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace MusicKG.Workflow.Settings
{
    public class KEProcessorsSettings
    {
        public Dictionary<string, KEProcessorSettings> ProcessorSettings { get; private set; }

        public KEProcessorsSettings()
        {
            this.ProcessorSettings = new Dictionary<string, KEProcessorSettings>();
        }

        public void ParseFromConfiguration(IConfiguration configuration)
        {
            this.ProcessorSettings = configuration.GetSection("KEProcessors").GetChildren().Select(x =>
            {
                var result = new KEProcessorSettings();
                result.ParseFromConfiguration(x);
                return result;
            }).ToDictionary(y => y.ProcessorName);
        }
    }
}

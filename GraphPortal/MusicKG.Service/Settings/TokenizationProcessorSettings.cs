using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.Settings
{
    public class TokenizationProcessorSettings
    {
        private const string ServerUrlKey = "TokenizationProcessor:ServiceUrl";
        private const string LabelingTaskNameKey = "TokenizationProcessor:LabelingTaskName";
        private const string UseDictionaryKey = "TokenizationProcessor:UseDictionary";

        public string ServiceUrl { get; set; }

        public string LabelingTaskName { get; set; } = "tokenization";

        public bool UseDictionary { get; set; } = true;

        public void ParseFrom(IConfiguration configuration)
        {
            ServiceUrl = configuration.GetSection(ServerUrlKey)?.Value;
            LabelingTaskName = configuration.GetSection(LabelingTaskNameKey)?.Value;
            if (bool.TryParse(configuration.GetSection(UseDictionaryKey)?.Value, out bool useDictionary))
            {
                UseDictionary = useDictionary;
            }
        }
    }
}

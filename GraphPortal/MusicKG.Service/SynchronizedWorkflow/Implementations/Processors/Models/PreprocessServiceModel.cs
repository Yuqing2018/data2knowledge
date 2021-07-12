using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.Models
{
    public class PreprocessServiceModel
    {
        [JsonProperty("labeling_task")]
        public string LabelingTaskName { get; set; }

        [JsonProperty("paragraphs")]
        public IEnumerable<string> Paragraphs { get; set; }

        [JsonProperty("tokenization_dict")]
        public IEnumerable<string> TokenizationDict { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.Models
{
    public class TextSimilarityModel
    {
        public List<TextSimilarityItemModel> Items { get; set; }

        public string Guideline { get; set; }
    }

    public class TextSimilarityItemModel
    {
        public string Id { get; set; }

        public string TextSource { get; set; }

        public List<string> TextTargets { get; set; }
    }
}

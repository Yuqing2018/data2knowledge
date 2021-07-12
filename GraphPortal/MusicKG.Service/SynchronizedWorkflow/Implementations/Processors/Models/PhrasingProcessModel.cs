using System;
using System.Collections.Generic;
using System.Text;

namespace MusicKG.Service.SynchronizedWorkflow.Implementations.Processors.Models
{
    public class PhrasingProcessModel
    {
        public List<PhrasingItemProcessModel> Items { get; set; }

        public string Guideline { get; set; }
    }

    public class PhrasingItemProcessModel
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public List<string> SpanItems { get; set; }
    }
}

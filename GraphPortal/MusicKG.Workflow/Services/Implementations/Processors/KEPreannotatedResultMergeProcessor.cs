using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicKG.DataAccess.Enums;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Settings;

namespace MusicKG.Workflow.Services.Implementations.Processors
{
    /// <summary>
    /// Knowledge extraction preannotate result merge processor.
    /// </summary>
    public class KEPreannotatedResultMergeProcessor : KEWebApiProcessor
    {
        /// <summary>
        /// Constructor of Processor for Knowledge Extraction preannotate result merge.
        /// </summary>
        public KEPreannotatedResultMergeProcessor()
        {
            this.processorName = "KEPreannotatedResultMergeProcessor";
        }
    }
}

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
    /// Knowledge extraction prennoatate processor.
    /// </summary>
    public class KEPreannotateProcessor : KEWebApiProcessor
    {
        /// <summary>
        /// Constructor of Processor for Knowledge Extraction preannotate.
        /// </summary>
        public KEPreannotateProcessor()
        {
            this.processorName = "KEPreannotateProcessor";
        }
    }
}

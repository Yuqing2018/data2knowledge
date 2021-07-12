using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicKG.DataAccess;
using MusicKG.DataAccess.Enums;
using MusicKG.Service;
using MusicKG.Workflow.Models.ServiceModels;
using MusicKG.Workflow.Settings;

namespace MusicKG.Workflow.Services.Implementations.Processors
{
    /// <summary>
    /// Processor for Knowledge Extraction preprocess.
    /// </summary>
    public class KEPreprocessProcessor : KEWebApiProcessor
    {
        /// <summary>
        /// Constructor of Processor for Knowledge Extraction preprocess.
        /// </summary>
        public KEPreprocessProcessor() : base()
        {
            this.processorName = "KEPreprocessProcessor";
        }
    }
}

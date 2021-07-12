using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicKG.DataAccess.Enums;
using MusicKG.Workflow.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services
{
    public interface IProcessorProvider
    {
        void Initialize(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger);

        Task<DocumentProcessorServiceModel> ProcessAsync(string documentId, string stepId, DocumentStatusEnum status);
    }
}

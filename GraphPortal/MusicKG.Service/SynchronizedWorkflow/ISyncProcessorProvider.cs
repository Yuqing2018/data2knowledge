using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicKG.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.Service.SynchronizedWorkflow
{
    public interface ISyncProcessorProvider
    {
        void Initialize(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger);

        Task<(byte[] content, string contentType, long itemCount)> ProcessAsync(string workspaceId, byte[] content, string contentType);
    }
}

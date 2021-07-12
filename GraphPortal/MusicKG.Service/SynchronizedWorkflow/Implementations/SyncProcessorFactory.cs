using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Helpers;
using MusicKG.Service.Resources;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MusicKG.Service.SynchronizedWorkflow.Implementations
{
    public class SyncProcessorFactory : ISyncProcessorFactory
    {
        private readonly ILogger<SyncProcessorFactory> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Constructor of sycnchronized processor factory.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="configuration">Configuration.</param>
        public SyncProcessorFactory(ILogger<SyncProcessorFactory> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
        }

        public ISyncProcessorProvider CreateProcessor(string assemblyName, string className)
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                if (assembly == null)
                {
                    ErrorHelper.ThrowException(MusicKGMessages.PreannotationAssemblyNotFound, System.Net.HttpStatusCode.InternalServerError, assemblyName);
                }
                object processor = assembly.CreateInstance(className);
                if (processor == null)
                {
                    ErrorHelper.ThrowException(MusicKGMessages.PreannotationClassNotFound, System.Net.HttpStatusCode.InternalServerError, className);
                }

                var result = processor as ISyncProcessorProvider;
                if (result == null)
                {
                    ErrorHelper.ThrowException(MusicKGMessages.InvalidPreannotationProcessor, System.Net.HttpStatusCode.InternalServerError, className);
                }
                else
                {
                    result.Initialize(this.serviceProvider, this.configuration, this.logger);
                }

                return result;
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, $"Create processor '{className}' failed from Assembly '{assemblyName}'.");
                return null;
            }
        }
    }
}

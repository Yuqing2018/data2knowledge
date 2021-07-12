using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Services.Implementations
{
    /// <summary>
    /// Processor factory.
    /// </summary>
    public class ProcessorFactory : IProcessorFactory
    {
        private readonly ILogger<ProcessorFactory> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Constructor of processor factory.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="configuration">Configuration.</param>
        public ProcessorFactory(ILogger<ProcessorFactory> logger, 
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
        }

        /// <summary>
        /// Create processor instance.
        /// </summary>
        /// <param name="assemblyName">Processor assembly name.</param>
        /// <param name="className">Processor class name.</param>
        /// <returns>Processor instance.</returns>
        public IProcessorProvider CreateProcessor(string assemblyName, string className)
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                if (assembly == null)
                {
                    throw new FileNotFoundException($"Processor assembly {assemblyName} is not found.");
                }
                object processor = assembly.CreateInstance(className);
                if (processor == null)
                {
                    throw new TypeLoadException($"Processor class {className} is not found.");
                }

                var result = processor as IProcessorProvider;
                if (result == null)
                {
                    throw new InvalidCastException($"Processor class {className} must implement IProcessorProvider.");
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

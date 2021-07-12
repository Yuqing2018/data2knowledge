using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicKG.WebApi.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.Elasticsearch;
using Serilog.Configuration;

namespace MusicKG.WebApi.Extensions
{
    /// <summary>
    /// Logger builder extensions.
    /// </summary>
    public static class LoggerBuilderExtensions
    {
        /// <summary>
        /// Add customer logger.
        /// </summary>
        /// <param name="loggingBuilder">Logging builder</param>
        /// <param name="configuration">Configuration Settings.</param>
        /// <returns>Logging builder.</returns>
        public static ILoggingBuilder AddCustomerLogger(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            LoggingTypeSettings settings = new LoggingTypeSettings();
            settings.ParseFrom(configuration);

            if (settings.Serilog)
            {
                loggingBuilder = loggingBuilder.AddSerilog(new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger(), true);
            }

            return loggingBuilder;
        }
    }
}

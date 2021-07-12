using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Settings
{
    public class LoggingTypeSettings
    {
        public const string SerilogKey = "LoggingTypes:Serilog";

        /// <summary>
        /// True to enable Serilog.
        /// </summary>
        public bool Serilog { get; set; }

        public void ParseFrom(IConfiguration configuration)
        {
            this.Serilog = "true".Equals(configuration.GetSection(SerilogKey)?.Value?.ToLower());
        }
    }
}

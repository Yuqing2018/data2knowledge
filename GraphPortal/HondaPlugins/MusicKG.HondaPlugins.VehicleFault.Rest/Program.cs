using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MusicKG.HondaPlugins.VehicleFault.Rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHostBuilder(args).Run();
        }

        public static IWebHost WebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 .UseSerilog((hostingContext, logging) =>
                 {
                     logging.ReadFrom.Configuration(hostingContext.Configuration);
                 })
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     var env = hostingContext.HostingEnvironment;
                     config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                     config.AddEnvironmentVariables();
                 })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                })
                .UseStartup<Startup>()
                .Build();
    }
}

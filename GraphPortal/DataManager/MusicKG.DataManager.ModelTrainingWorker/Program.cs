using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicKG.Scheduler.Extensions;
using Serilog;
using System;

namespace MusicKG.DataManager.ModelTrainingWorker
{
    class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, logging) =>
                {
                    logging.ReadFrom.Configuration(hostingContext.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient().AddScheduler(hostContext.Configuration.GetSection("Scheduler"),
                            (service, configuration) =>
                            {
                                return service.AddPlugins(hostContext.Configuration);
                            })
                        .AddRunner();
                });
    }

}

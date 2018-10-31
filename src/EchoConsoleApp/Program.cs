using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EchoConsoleApp
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static async Task Main(string[] args)
        {
            var environmentName = EnvironmentName.Development;

            var hostBuilder = new HostBuilder()
                //                // Add configuration, logging, ...
                .UseEnvironment(environmentName)
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    hostContext.HostingEnvironment.ApplicationName = "Echo";
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ActorSystemHostingService>();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}

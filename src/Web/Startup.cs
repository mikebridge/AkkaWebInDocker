using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        
        }


        // ReSharper disable once UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEchoActorSystem>(sp => CreateEchoActorSystem());
            services.AddMvc();
            
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();

        }

        private static EchoActorSystem CreateEchoActorSystem()
        {
            // TODO: Replace this
            var echoHost = GetEchoHostName("localhost", 5002);
            var internalHost = GetInternalHostName("localhost", 5001);
            var externalHost = GetExternalHostName("localhost", 5001);

//            var publicHost = Environment.GetEnvironmentVariable("API_EXTERNAL_HOST") ?? "localhost";
//            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable("API_PORT"), out var publicPort);
//            publicPort = portSuccess ? publicPort : 5001;
//            var remoteHost = Environment.GetEnvironmentVariable("ECHO_HOST") ?? "localhost";
//            portSuccess = int.TryParse(Environment.GetEnvironmentVariable("ECHO_PORT"), out var remotePort);
//            remotePort = portSuccess ? remotePort : 5002;

            return EchoActorSystem.Create(externalHost, internalHost, echoHost);
        }

        private static HostAndPort GetEchoHostName(string defaultHost, int defaultPort)
        {
            var internalHostName = Environment.GetEnvironmentVariable("ECHO_HOST") ?? defaultHost;
            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable("ECHO_PORT"), out var internalPort);
            internalPort = portSuccess ? internalPort : defaultPort;
            var internalHost = new HostAndPort(internalHostName, internalPort);
            return internalHost;
        }

        private static HostAndPort GetInternalHostName(string defaultHost, int defaultPort)
        {
            var internalHostName = Environment.GetEnvironmentVariable("API_INTERNAL_HOST") ?? defaultHost;
            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable("API_INTERNAL_PORT"), out var internalPort);
            internalPort = portSuccess ? internalPort : defaultPort;
            var internalHost = new HostAndPort(internalHostName, internalPort);
            return internalHost;
        }

        private static HostAndPort GetExternalHostName(string defaultHost, int defaultPort)
        {
            var externalHostName = Environment.GetEnvironmentVariable("API_EXTERNAL_HOST") ?? defaultHost;
            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable("API_EXTERNAL_PORT"), out var externalPort);
            externalPort = portSuccess ? externalPort : defaultPort;
            return new HostAndPort(externalHostName, externalPort);
        }


    }
}

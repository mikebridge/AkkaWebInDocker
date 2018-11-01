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
            // This is where the echo akka system is
            var echoHost = GetEchoHostName("localhost", 5002);
            
            // What we bind to internally
            var internalHost = GetInternalHostName("localhost", 5001);
            
            // What we present ourselves to via NAT/Docker
            var externalHost = GetExternalHostName("localhost", 5001);

            return EchoActorSystem.Create(externalHost, internalHost, echoHost);
        }

        

        private static HostAndPort GetEchoHostName(string defaultHost, int defaultPort)
        {
            return GetHostAndPort(defaultHost, defaultPort, "ECHO_HOST", "ECHO_PORT");
        }

        private static HostAndPort GetInternalHostName(string defaultHost, int defaultPort)
        {
            return GetHostAndPort(defaultHost, defaultPort, "API_INTERNAL_HOST", "API_INTERNAL_PORT");
        }

        private static HostAndPort GetExternalHostName(string defaultHost, int defaultPort)
        {
            return GetHostAndPort(defaultHost, defaultPort, "API_EXTERNAL_HOST", "API_EXTERNAL_PORT");
        }

        private static HostAndPort GetHostAndPort(
            string defaultHost,
            int defaultPort,
            string hostVar,
            string portVar)
        {
            var internalHostName = Environment.GetEnvironmentVariable(hostVar) ?? defaultHost;
            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable(portVar), out var internalPort);
            internalPort = portSuccess ? internalPort : defaultPort;
            var internalHost = new HostAndPort(internalHostName, internalPort);
            return internalHost;
        }



    }
}

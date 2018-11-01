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

            var publicHost = Environment.GetEnvironmentVariable("API_PUBLIC_HOST") ?? "localhost";
            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable("API_PORT"), out var publicPort);
            publicPort = portSuccess ? publicPort : 5001;
            var remoteHost = Environment.GetEnvironmentVariable("ECHO_HOST") ?? "localhost";
            portSuccess = int.TryParse(Environment.GetEnvironmentVariable("ECHO_PORT"), out var remotePort);
            remotePort = portSuccess ? remotePort : 5002;

            return EchoActorSystem.Create(publicHost, publicPort, remoteHost, remotePort);
        }

    }
}

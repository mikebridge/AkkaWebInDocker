using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using EchoConsoleApp.Actors;
using Microsoft.Extensions.Hosting;

namespace EchoConsoleApp
{
    public class ActorSystemHostingService : IHostedService //, IDisposable
    {
        private ActorSystem _system;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var hoconConfig = HoconConfig();
            Console.WriteLine(hoconConfig);

            var config = ConfigurationFactory.ParseString(hoconConfig);
            _system = ActorSystem.Create("EchoConsoleApp", config);
            _system.ActorOf(EchoActor.Props(), "EchoActor");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _system.Terminate();
        }

        private static string HoconConfig()
        {
            var externalHost = GetExternalHostName("localhost", 5002);
            var internalHost = GetInternalHostName("localhost", 5002);

            // As per https://getakka.net/articles/remoting/transports.html,

            Console.WriteLine($"ActorSystemHostingSerice CONFIG");
            Console.WriteLine($"         bind to: {internalHost}");
            Console.WriteLine($" public-hostname: {externalHost}");
            return @"akka {  
                stdout-loglevel = DEBUG
                loglevel = DEBUG
                log-config-on-start = on
                actor {
                    provider = remote
                }
                remote {
                    dot-netty.tcp {
                        bind-hostname = " + internalHost.Host + @"
                        bind-port = " + internalHost.Port + @"                         
                        hostname = " + externalHost.Host + @"
                        port = " + externalHost.Port + @"
                    }
                }
            }";
            //                        public-hostname = " + publicHostname + @"
        }


        private static HostAndPort GetInternalHostName(string defaultHost, int defaultPort)
        {
            var internalHostName = Environment.GetEnvironmentVariable("ECHO_INTERNAL_HOST") ?? defaultHost;
            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable("ECHO_PORT"), out var internalPort);
            internalPort = portSuccess ? internalPort : defaultPort;
            var internalHost = new HostAndPort(internalHostName, internalPort);
            return internalHost;
        }

        private static HostAndPort GetExternalHostName(string defaultHost, int defaultPort)
        {
            var externalHostName = Environment.GetEnvironmentVariable("ECHO_EXTERNAL_HOST") ?? defaultHost;
            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable("ECHO_EXTERNAL_PORT"), out var externalPort);
            externalPort = portSuccess ? externalPort : defaultPort;
            return new HostAndPort(externalHostName, externalPort);
        }



    }
}

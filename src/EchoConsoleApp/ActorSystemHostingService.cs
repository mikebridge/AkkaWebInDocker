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
            // TODO: Get this from the environment
            // this will be localhost in dev, the docker container name in docker, and the public api in production

            var publicHostName = Environment.GetEnvironmentVariable("ECHO_PUBLIC_HOST") ?? "localhost";
            var hostName = Environment.GetEnvironmentVariable("ECHO_HOST") ?? "localhost";
            var portSuccess = int.TryParse(Environment.GetEnvironmentVariable("ECHO_PORT"), out var localport);
            localport = portSuccess ? localport : 5002;
            //var remoteHost = 

            //var publicHostname = "echo";
            //var publicHostname = "localhost";
            var hoconConfig = HoconConfig(publicHostName, hostName, localport);
            Console.WriteLine(hoconConfig);
            var config = ConfigurationFactory.ParseString(hoconConfig);
            Console.WriteLine("Initializing...");
            _system = ActorSystem.Create("EchoConsoleApp", config);
            _system.ActorOf(EchoActor.Props(), "EchoActor");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _system.Terminate();
        }

//        public void Dispose()
//        {
//            //throw new NotImplementedException();
//        }

        private static string HoconConfig(String publicHostname, String hostname, int port)
        {
            // As per https://getakka.net/articles/remoting/transports.html,
            // we are listening on all network interfaces (hostname=0.0.0.0)
            // but we are telling remote systems to access us via publicHostname (public-hostname) variable.
            // TODO: bring these in from the environment
            //const string hostname = "0.0.0.0"; // this doesn't seem to work.

            Console.WriteLine($"ActorSystemHostingSerice CONFIG");
            Console.WriteLine($"         bind to: {hostname}:{port}");
            Console.WriteLine($" public-hostname: {publicHostname}:{port}");
            return @"akka {  
                stdout-loglevel = DEBUG
                loglevel = DEBUG
                log-config-on-start = on
                actor {
                    provider = remote
                }
                remote {
                    dot-netty.tcp {
                        bind-port = " + port + @" 
                        bind-hostname = " + hostname + @" 
                        hostname = " + publicHostname + @"
                        port = " + port + @"
                    }
                }
            }";
            //                        public-hostname = " + publicHostname + @"
        }

    }
}

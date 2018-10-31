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

            //var publicHostname = "echo";
            var publicHostname = "localhost";
            var config = ConfigurationFactory.ParseString(HoconConfig(publicHostname));
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

        private static string HoconConfig(String publicHostname)
        {
            // As per https://getakka.net/articles/remoting/transports.html,
            // we are listening on all network interfaces (hostname=0.0.0.0)
            // but we are telling remote systems to access us via publicHostname (public-hostname) variable.
            // TODO: bring these in from the environment
            //const string hostname = "0.0.0.0"; // this doesn't seem to work.

            const string hostname = "localhost";
            const string port = "5002";

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
                        port = " + port + @" 
                        hostname = " + hostname + @" 
                        bind-hostname = " + publicHostname + @"
                        bind-port = " + port + @"
                    }
                }
            }";
            //                        public-hostname = " + publicHostname + @"
        }

    }
}

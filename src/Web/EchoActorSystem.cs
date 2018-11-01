using System;

using System.Threading.Tasks;

using Akka.Actor;

using EchoConsoleApp.Messages;
using Microsoft.Extensions.Logging;

namespace Web
{
    public class EchoActorSystem: IEchoActorSystem, IDisposable
    {
        private readonly ActorSystem _actorSystem;
        private readonly string _remoteHost;
        private readonly int _remotePort;

        private EchoActorSystem(ActorSystem actorSystem, string remoteHost, int remotePort)
        {
            _actorSystem = actorSystem;
            _remoteHost = remoteHost;
            _remotePort = remotePort;
            
        }

        public static EchoActorSystem Create(string publicHostName, int publicPort, string remoteHost, int remotePort)
        {            

            const string hostName = "localhost"; // listen on this locally
            const int localPort = 5001;

            var config = @"akka {  
    actor {
        provider = remote
    }
    remote {
        outbound-max-restarts = 1
        dot-netty.tcp {
            bind-port = " + localPort + @"
            bind-hostname = " + hostName + @"
            hostname = " + publicHostName + @"
            port = " + publicPort + @"
        }
    }
}";
            //            public-hostname = " + publicHostname + @"
            // bind-hostname = " + host + @" # internal (bind) hostname
            // bind-port = " + port + @" # internal (bind) port


            var akkaConfig = Akka.Configuration.ConfigurationFactory.ParseString(config);

            var actorSystem = ActorSystem.Create("EchoActorSystemClient", akkaConfig);
            return new EchoActorSystem(actorSystem, remoteHost, remotePort);
        }
        
        public Task Send(string msg)
        {            
            var actorPath = "akka.tcp://EchoConsoleApp@" + _remoteHost+
                            @":" + _remotePort + "/user/EchoActor";
            Console.WriteLine("Connecting to " + actorPath);
            var actor = _actorSystem.ActorSelection(actorPath);
            actor.Tell(new EchoMessage(msg));
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _actorSystem.Terminate().Wait();
        }

    }
}

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
        private readonly HostAndPort _remoteHost;

        private EchoActorSystem(ActorSystem actorSystem, HostAndPort remoteHost)
        {
            _actorSystem = actorSystem;
            _remoteHost = remoteHost;
            
        }

        //public static EchoActorSystem Create(string publicHostName, int publicPort, string remoteHost, int remotePort)
        public static EchoActorSystem Create(
            HostAndPort externalHost, 
            HostAndPort internalHost,
            HostAndPort remoteHost)
        {            

            const string hostName = "localhost"; // listen on this locally
            const int localPort = 5001;
            
            // https://doc.akka.io/docs/akka/2.5.4/java/remoting.html#akka-behind-nat-or-in-a-docker-container
            var config = @"akka {  
    actor {
        provider = remote
    }
    remote {
        outbound-max-restarts = 1
        dot-netty.tcp {
            bind-hostname = " + internalHost.Host + @" # internal (bind) hostname
            bind-port = " + internalHost.Port + @" # internal (bind) port            
            hostname = " + externalHost.Host + @" # external (logical) hostname
            port = " + externalHost.Port + @" # external (logical) port
        }
    }
}";
            //            public-hostname = " + publicHostname + @"

            var akkaConfig = Akka.Configuration.ConfigurationFactory.ParseString(config);

            var actorSystem = ActorSystem.Create("EchoActorSystemClient", akkaConfig);
            return new EchoActorSystem(actorSystem, remoteHost);
        }
        
        public Task Send(string msg)
        {            
            var actorPath = $"akka.tcp://EchoConsoleApp@{_remoteHost.Host}:{_remoteHost.Port}/user/EchoActor";
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

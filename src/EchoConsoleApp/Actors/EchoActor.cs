using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Event;
using EchoConsoleApp.Messages;

namespace EchoConsoleApp.Actors
{
    public class EchoActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new EchoActor());
        }

        public EchoActor()
        {
            _log.Debug("Creating EchoActor");
            Receive<EchoMessage>(message =>
            {
                _log.Debug("Received message: " + message.Text);
                // right now nothing's configured to listen for this message.
                //var response = new ResponseMessage(FormatReply(message.Text));
                //Sender.Tell(response);
            });
        }

        public static string FormatReply(String original)
        {
            return "ECHO: " + original;
        }
    }
}

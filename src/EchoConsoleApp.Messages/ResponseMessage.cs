using System;
using System.Dynamic;

namespace EchoConsoleApp.Messages
{
    public class ResponseMessage
    {
        public ResponseMessage(String text)
        {
            Text = text;
        }

        public string Text { get; }

    }
}

using System;
using System.Dynamic;

namespace EchoConsoleApp.Messages
{
    public class EchoMessage
    {
        public EchoMessage(String text)
        {
            Text = text;
        }

        public string Text { get; }

    }
}

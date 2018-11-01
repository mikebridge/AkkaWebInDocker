using System;

namespace EchoConsoleApp
{
    public class HostAndPort
    {
        public HostAndPort(string host, int port)
        {
            Host = host;
            Port = port;
        }
        public string Host { get; }

        public int Port { get; }

        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}

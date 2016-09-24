using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 2)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }
            var ip = IPAddress.Parse(args[0]);
            int port = int.Parse(args[1]);

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);

            var currentEndPoint = new IPEndPoint(IPAddress.Loopback, port);
        }
    }
}

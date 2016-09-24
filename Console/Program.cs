using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cons
{
    class Program
    {
        static void Main(string[] args)
        {
            var ip = args.Length > 0 ? args[0] : "127.0.0.1";
            var port = args.Length > 1 ? Convert.ToInt32(args[1]) : 9050;
            UdpClient _server = new UdpClient(ip, port);
            var _sender = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                Console.WriteLine("log or create");
                var s = Console.ReadLine();
                if (s == "log")
                {
                    Console.WriteLine("Enter login");
                    var log = Console.ReadLine();
                    Console.WriteLine("Enter password");
                    var pass = Console.ReadLine();

                    byte[] data = new byte[1024];
                    string stringData;

                    string passHash = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pass)));

                    data = Encoding.UTF8.GetBytes("authorizate+" + log + "+" + passHash);
                    _server.Send(data, data.Length);

                    // Get answer from server
                    data = _server.Receive(ref _sender);

                    stringData = Encoding.UTF8.GetString(data, 0, data.Length);

                    if (stringData != "success")
                        Console.WriteLine("Invalid user name or password. Try again.");
                    else
                        break;
                    continue;
                }
                if (s == "create")
                {
                    Console.WriteLine("Enter login");
                    var log = Console.ReadLine();
                    Console.WriteLine("Enter password");
                    var pass = Console.ReadLine();

                    byte[] data = new byte[1024];
                    string stringData;

                    string passHash = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pass)));

                    data = Encoding.UTF8.GetBytes("create+" + log + "+" + passHash);
                    _server.Send(data, data.Length);

                    // Get answer from server
                    data = _server.Receive(ref _sender);

                    stringData = Encoding.UTF8.GetString(data, 0, data.Length);

                    if (stringData != "success")
                        Console.WriteLine("Error");
                    else
                        break;
                    continue;
                }
                
            }

            Console.WriteLine("OK");

            while (true)
            {
                var s = Console.ReadLine();
                if (s == "exit")
                    return;
                else
                {
                    var data = Encoding.UTF8.GetBytes(s);
                    _server.Send(data, data.Length);
                    while (true)
                    {
                        data = _server.Receive(ref _sender);
                        var str = Encoding.UTF8.GetString(data);
                        if (str == "end")
                            break;
                        Console.WriteLine(str);
                    }
                }
            }
        }
    }
}

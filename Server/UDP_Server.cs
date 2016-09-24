using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class UDP_Server
    {
        Dictionary<string, Tuple<string, string>> _users = new Dictionary<string, Tuple<string, string>>();

        Socket _srvSock;

        public UDP_Server()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
            
            _srvSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            _srvSock.Bind(ipep);
            Console.WriteLine("Waiting for connection");

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            
            LoadUsers();
        }

        /// <summary>
        /// Save users
        /// </summary>
        private void SaveUsers()
        {
            using (StreamWriter writer = new StreamWriter("users"))
            {
                foreach (KeyValuePair<string, Tuple<string, string>> t in _users)
                {
                    writer.WriteLine(t.Key + " " + t.Value.Item1);
                }
            }
        }

        /// <summary>
        /// Load users
        /// </summary>
        private void LoadUsers()
        {
            _users = new Dictionary<string, Tuple<string, string>>();
            if (!File.Exists("users"))
                return;
            using (StreamReader reader = new StreamReader("users"))
            {
                while(true)
                {
                    string s = reader.ReadLine();
                    string[] arr = s.Split(' ');
                    _users.Add(arr[0], new Tuple<string, string>(arr[1], ""));
                    if (reader.EndOfStream)
                        return;
                }
            }
        }

        public void Send(string message, EndPoint _remote)
        {
            var data = Encoding.UTF8.GetBytes(message);
            _srvSock.SendTo(data, data.Length, SocketFlags.None, _remote);
        }

        /// <summary>
        /// Start listen commands
        /// </summary>
        public string Listen(ref EndPoint _remote)
        {
            int recv;
            byte[] data = new byte[1024];

            data = new byte[1024];
            // Get data from client
            recv = _srvSock.ReceiveFrom(data, ref _remote);

            Console.Write("Message recieved from {0}:", _remote.ToString());
            string[] arr = Encoding.UTF8.GetString(data, 0, recv).Split('+');
            string command = arr[0];

            if (command == "authorizate")
            {
                string user = arr[1];
                string passHash = arr[2];
                if (_users.ContainsKey(user) && _users[user].Item1 == passHash)
                {
                    _users[user] = new Tuple<string,string>(passHash, _remote.ToString());
                    data = Encoding.UTF8.GetBytes("success");
                    _srvSock.SendTo(data, data.Length, SocketFlags.None, _remote);
                    Console.WriteLine(string.Format("User '{0}' conencted", user));
                }
                else
                {
                    data = Encoding.UTF8.GetBytes("error");
                    _srvSock.SendTo(data, data.Length, SocketFlags.None, _remote);
                    Console.WriteLine(string.Format("User '{0}' invalid login or password", user));
                }
                return "";
            }

            if (command == "create")
            {
                string user = arr[1];
                string passHash = arr[2];
                if (!_users.ContainsKey(user))
                {
                    _users.Add(user, new Tuple<string, string>(passHash, _remote.ToString()));
                    SaveUsers();
                    Send("success", _remote);
                }
                else
                {
                    data = Encoding.UTF8.GetBytes("user is created before");
                    _srvSock.SendTo(data, data.Length, SocketFlags.None, _remote);
                }
                return "";
            }
            return command;
        }
    }
}

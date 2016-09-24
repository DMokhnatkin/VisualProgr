using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Web.Script.Serialization;

namespace TurtleServer
{
    class Server
    {
        private struct LoginData
        {
            public string message;
            public string login;
            public string password;
        }
        const string GREETING_MESSAGE = "started";
        const string CLOSE_MESSAGE = "closed";
        const string LOGIN_MESSAGE = "auth_required";
        const string LOGIN_SUCCESS_MESSAGE = "auth_success";
        const string LOGOUT_MESSAGE = "logout";
        private Udp server;
        private Dictionary<string, Client> clients;

        public Server()
        {
            clients = new Dictionary<string, Client>();
        }

        public void start()
        {
            server = new Udp();
            UserManager.load();
            try
            {
                server.start();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error] " + e.Message);
                Console.ReadKey();
                Environment.Exit(1);
            }

            string input = "";
            while (true)
            {
                server.listenState.Reset();
                input = server.getReceivedMessage();
                string address = server.getClientAddress();
                if (!clients.ContainsKey(address))
                {
                    Client newClient = new Client();
                    clients.Add(address, newClient);
                }
                string header = clients[address].runCommand("help") + "\n" + clients[address].runCommand("status");
                switch (input)
                {
                    case GREETING_MESSAGE:
                        if (UserManager.isAuthorized(address))
                        {

                            server.sendMessage(header);
                        }
                        else
                        {
                            server.sendMessage(LOGIN_MESSAGE);
                        }
                        break;

                    case LOGOUT_MESSAGE:
                        if (UserManager.isAuthorized(address))
                        {
                            UserManager.Deauthorize(address);
                        }
                        clients.Remove(address);
                        server.sendMessage(LOGOUT_MESSAGE);
                        break;

                    default:
                        if (!UserManager.isAuthorized(address))
                        {
                            var serializer = new JavaScriptSerializer();
                            try
                            {
                                LoginData loginData = serializer.Deserialize<LoginData>(input);
                                if (loginData.message == "register")
                                {
                                    try
                                    {
                                        UserManager.Register(loginData.login, loginData.password);
                                    }
                                    catch (ApplicationException e)
                                    {
                                        Console.WriteLine("[{0}] Error: {1}", address, e.Message);
                                        server.sendMessage(e.Message);
                                    }
                                }
                                try
                                {
                                    bool result = UserManager.Authorize(loginData.login, loginData.password, address);
                                    if (result)
                                    {
                                        server.sendMessage(LOGIN_SUCCESS_MESSAGE);
                                        server.sendMessage(header);
                                    }
                                    else
                                    {
                                        server.sendMessage("Wrong username or password.");
                                    }
                                }
                                catch (KeyNotFoundException e)
                                {
                                    Console.WriteLine("[{0}] Error: {1}", address, e.Message);
                                    server.sendMessage(e.Message);
                                }
                                catch (ApplicationException e)
                                {
                                    Console.WriteLine("[{0}] Error: {1}", address, e.Message);
                                    server.sendMessage(e.Message);
                                }
                            }
                            catch (ArgumentException e)
                            {
                                server.sendMessage("Login failed");
                            }
                            catch (InvalidOperationException e)
                            {

                            }
                        }
                        else
                        {
                            if (input != CLOSE_MESSAGE)
                            {
                                string output = clients[address].runCommand(input);

                                Console.WriteLine("[{0}] {1}", address, input);
                                if (input != "exec")
                                {
                                    Console.WriteLine("[Server] sent output to {0}: {1}", address, output);
                                    server.sendMessage(output);
                                }
                                else
                                {
                                    Console.WriteLine("[Server] sent exec message to {0}", address);
                                    server.sendMessage("Execution completed");
                                }
                            }
                            else
                            {
                                Console.WriteLine("[{0}] disconnected", address);
                            }
                        }
                        break;
                }
                input = "";
                server.listenState.WaitOne();
            }


        }
    }
}

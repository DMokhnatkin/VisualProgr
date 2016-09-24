using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {

        struct Parameters
        {
            public EndPoint remote;
            public string message;
            public Main.Nodes.NodeTypesManager nodeTypes;
            public MyConsole cons;
            public Main.Sceme.Scheme scheme;
            public UDP_Server serv;

            public Parameters(EndPoint _remote, string _message, Main.Nodes.NodeTypesManager _nodeTypes, MyConsole _cons, Main.Sceme.Scheme _scheme, UDP_Server _serv)
            {
                remote = _remote;
                message = _message;
                nodeTypes = _nodeTypes;
                cons = _cons;
                scheme = _scheme;
                serv = _serv;
            }
        }

        static IAsyncResult BeginHandle(object info, AsyncCallback callback, object state)
        {
            Action<object> t = new Action<object>(Handle);
           return t.BeginInvoke(info, null, null);
        }

        static void EndHandle(IAsyncResult res)
        {
            var del = ((AsyncResult)res).AsyncDelegate;
            ((Action<object>)del).EndInvoke(res);
        }

        static void Handle(object info)
        {
            var par = (Parameters)info;
            try
            {
                var t = par.cons.ParseCommand(par.message, par.nodeTypes, par.scheme, par.remote);
                if (!t)
                    par.serv.Send("Invalid command", par.remote);
            }
            catch (Exception e)
            {
                par.cons.PrintError(e.Message, par.remote);
            }
            par.serv.Send("end", par.remote);
        }

        static void Main(string[] args)
        {
            UDP_Server serv = new UDP_Server();

            List<Assembly> _assemblys = new List<Assembly>();
            _assemblys.Add(Assembly.LoadFrom(@"StandartNodes.dll"));
            Main.Nodes.NodeTypesManager nodeTypes = new Main.Nodes.NodeTypesManager(_assemblys);

            Main.Sceme.Scheme scheme = new Main.Sceme.Scheme("test");

            MyConsole cons = new MyConsole(serv);

            while (true)
            {
                EndPoint remote = new IPEndPoint(IPAddress.Any, 9050);
                var comm = serv.Listen(ref remote);
                if (comm != "")
                {
                    Parameters par = new Parameters(remote, comm, nodeTypes, cons, scheme, serv);
                    //ParameterizedThreadStart paramThread = new ParameterizedThreadStart(Handle);
                    /*Thread thread = new Thread(paramThread);
                    thread.Start(par);*/
                    var t = BeginHandle(par, null, null);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server
{
    public class MyConsole
    {
        UDP_Server _serv;

        public MyConsole(UDP_Server serv)
        {
            _serv = serv;
        }

        public void PrintSuccess(string text, EndPoint _remote)
        {
            Console.WriteLine("[OK]" + text);
            _serv.Send("[OK]" + text, _remote);
        }

        public void PrintError(string text, EndPoint _remote)
        {
            Console.WriteLine("[ERROR]" + text);
            _serv.Send("[ERROR]" + text, _remote);
        }

        public void Print(string text, EndPoint _remote)
        {
            Console.WriteLine(text);
            _serv.Send(text, _remote);
        }

        public void PrintHelp(Main.Nodes.NodeTypesManager nodeTypes, EndPoint _remote)
        {
            Print("[HELP]", _remote);
            Print("+ create <type_name> -node_name", _remote);
            Print("<group_name> - group name which contains nodeType", _remote);
            Print("<type_name> - name of nodeType", _remote);
            Print("-node_name - name of node(optional)", _remote);
            Print("available node types:", _remote);
            foreach (Main.Nodes.NodeType t in nodeTypes.GetAll())
            {
                Print("     " + t.FullName, _remote);
            }

            Print("+ print -node_name", _remote);
            Print("-node_name - name of node(optional)", _remote);
            Print("Print information about node", _remote);

            Print("+ connect <node1>.<port1> <node2>.<port2>", _remote);
            Print("<node1> - name of first node to connect", _remote);
            Print("<port1> - name of first port to connect", _remote);
            Print("<node2> - name of second node to connect", _remote);
            Print("<port2> - name of second port to connect", _remote);

            Print("+ set <node>.<port>", _remote);
            Print("Set constant to input port", _remote);
            Print("<node> - name of node to set constant", _remote);
            Print("<port> - name of Input port to set constant", _remote);

            Print("**************", _remote);
        }

        public bool ParseCommand(string command, Main.Nodes.NodeTypesManager nodeTypes, Main.Sceme.Scheme scheme, EndPoint _remote)
        {
            Regex reg = new Regex(@"^create ([^ ]+)(?: -([^ ]+)){0,1}$");
            Match match = reg.Match(command);
            bool flag = false;
            if (match.Success)
            {
                flag = true;
                try
                {
                    string nodeName;
                    if (match.Groups[3].Value != "")
                        nodeName = match.Groups[3].Value;
                    else
                        nodeName = null;
                    var newNode = scheme.CreateNode(nodeTypes.GetNodeType(match.Groups[1].ToString()), nodeName);
                    PrintSuccess(string.Format("Node '{0}' created", newNode.Name), _remote);
                }
                catch (Exception e)
                {
                    PrintError(e.Message, _remote);
                }
            }

            reg = new Regex(@"^print(?: -([^ ]+)){0,1}$");
            match = reg.Match(command);
            if (match.Success)
            {
                flag = true;
                try
                {
                    var nodes = scheme.Nodes;
                    foreach (Main.Nodes.Node node in nodes)
                    {
                        if (match.Groups[1].Value != "")
                            if (node.Name != match.Groups[1].Value)
                                continue;
                        Print(string.Format(">>'{0}' of type '{1}'", node.Name, node.NodeType.Name), _remote);
                        Print("inputs:", _remote);
                        var ports = node.PortManager.GetPorts(Main.Ports.PortType.Input);
                        foreach (Main.Ports.Port port in ports)
                        {
                            if (Main.Connections.ConnectionManager.Connected(port))
                            {
                                Main.Ports.Port _conenctedTo = Main.Connections.ConnectionManager.GetAllConnections(port)[0];
                                Print(string.Format("   {0} : connected to '{1}.{2}'",
                                      port.Name, _conenctedTo.Parent.Name, _conenctedTo.Name), _remote);
                            }
                            else
                                Print(string.Format("   {0} : value = {1}", port.Name, Main.Values.ValueManager.GetVal(port)), _remote);
                        }
                        Print("outputs:", _remote);
                        var outputs = node.PortManager.GetPorts(Main.Ports.PortType.Output);
                        foreach (Main.Ports.Port port in outputs)
                        {
                            Print(string.Format("   {0} : value(calculated) = {1}", port.Name, Main.Values.ValueManager.GetVal(port)), _remote);
                        }
                    }
                }
                catch (Exception e)
                {
                    PrintError(e.Message, _remote);
                }
            }

            reg = new Regex(@"^connect ([^ .]+).([^ .]+) ([^ .]+).([^ .]+)$");
            match = reg.Match(command);
            if (match.Success)
            {
                flag = true;
                try
                {
                    Main.Nodes.Node node1 = scheme.Nodes.Find((x) => x.Name == match.Groups[1].Value);
                    Main.Ports.Port port1 = node1.PortManager.GetPort(match.Groups[2].Value);
                    Main.Nodes.Node node2 = scheme.Nodes.Find((x) => x.Name == match.Groups[3].Value);
                    Main.Ports.Port port2 = node2.PortManager.GetPort(match.Groups[4].Value);
                    scheme.Connect(port1, port2);
                    PrintSuccess(string.Format("Conenction between '{0}.{1}' and '{2}.{3}' created",
                        node1.Name, port1.Name, node2.Name, port2.Name), _remote);
                }
                catch (Exception e)
                {
                    PrintError(e.Message, _remote);
                }
            }

            reg = new Regex(@"^set ([^ .]+).([^ .]+) (True|False|true|false)$");
            match = reg.Match(command);
            if (match.Success)
            {
                flag = true;
                try
                {
                    Main.Nodes.Node node1 = scheme.Nodes.Find((x) => x.Name == match.Groups[1].Value);
                    Main.Ports.Port port1 = node1.PortManager.GetPort(match.Groups[2].Value);
                    Main.Values.ValueManager.SetValue(port1, Convert.ToBoolean(match.Groups[3].Value));
                    PrintSuccess("Value changed", _remote);
                }
                catch (Exception e)
                {
                    PrintError(e.Message, _remote);
                }
            }

            reg = new Regex(@"^help$");
            match = reg.Match(command);
            if (match.Success)
            {
                flag = true;
                PrintHelp(nodeTypes, _remote);
            }

            if (!flag)
                PrintError("No such command", _remote);
            return true;
        }
    }
}

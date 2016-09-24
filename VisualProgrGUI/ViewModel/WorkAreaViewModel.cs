using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using VisualProgrGUI.ViewModel.Commands;
using System.Linq;
using VisualProgrGUI.behavior;
using VisualProgrGUI.ViewModel.NodeTypes;
using System;
using Main.Nodes;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Windows.Data;

namespace VisualProgrGUI.ViewModel
{
    public class WorkAreaViewModel : DependencyObject, IDropable
    {
        /// <summary>
        /// Nodes viewmodels in scene
        /// </summary>
        public ObservableCollection<NodeViewModel> Nodes { get; private set; }  

        public Main.Sceme.Scheme Scheme { get; private set; }

        /// <summary>
        /// Connections in work area
        /// </summary>
        public ObservableCollection<object> Connections { get; private set; }

        /// <summary>
        /// Node types
        /// </summary>
        public NodeTypesManager NodeTypes { get; private set; }

        /// <summary>
        /// Create new connection
        /// </summary>
        /// <returns>Connection was created or not</returns>
        public bool CreateConnection(PortViewModel first, PortViewModel second)
        {
            try
            {
                Scheme.Connect(first.Port, second.Port);
                ConnectionViewModel conn = new ConnectionViewModel();

                Binding bind = new Binding("Hotspot");
                bind.Source = first;
                BindingOperations.SetBinding(conn, ConnectionViewModel.Hotspot1Property, bind);

                bind = new Binding("Hotspot");
                bind.Source = second;
                BindingOperations.SetBinding(conn, ConnectionViewModel.Hotspot2Property, bind);

                Connections.Add(conn);
                ConsViewModel.Print(string.Format("Connection created beetwen {0} and {1}", first.Port, second.Port));
            }
            catch (Exception e)
            {
                ConsViewModel.PrintError(e.Message);
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Create new node
        /// </summary>
        /// <param name="debug">Print result in console</param>
        NodeViewModel CreateNode(Main.Nodes.NodeType type, double left, double right, bool debug, string name = null)
        {
            NodeViewModel inst = new NodeViewModel(Scheme.CreateNode(type, name), left, right, this);
            Nodes.Add(inst);

            if (debug) 
                ConsViewModel.Print(string.Format("Node ({0}) added", inst.NodeModel));
            return inst;
        }

        public WorkAreaViewModel(NodeTypesManager _nodeTypes)
        {
            Scheme = new Main.Sceme.Scheme("test");
            Nodes = new ObservableCollection<NodeViewModel>();
            Connections = new ObservableCollection<object>();
            NodeTypes = _nodeTypes;
        }

        /// <summary>
        /// Calculate all values in scene
        /// </summary>
        public void CalculateScene()
        {
            foreach (NodeViewModel node in Nodes)
                node.Update();
        }

        /// <summary>
        /// Clear all nodes
        /// </summary>
        public void ClearNodes()
        {
            Nodes.Clear();
            Connections.Clear();
            Scheme.Clear();
        }

        /// <summary>
        /// Serialize object
        /// </summary>
        public void Save(Stream stream)
        {
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Scheme");
            writer.WriteAttributeString("name", Scheme.Name);
            foreach (NodeViewModel node in Nodes)
            {
                writer.WriteStartElement("node");
                writer.WriteAttributeString("name", node.Name);
                writer.WriteAttributeString("nodeType", (string)node.NodeModel.GetType().GetField("typeName").GetValue(null));
                writer.WriteAttributeString("x", node.Left.ToString());
                writer.WriteAttributeString("y", node.Top.ToString());
                foreach (Main.Ports.Port port in node.NodeModel.PortManager.GetPorts(Main.Ports.PortType.Input))
                {
                    writer.WriteStartElement(port.Name);
                    if (!port.IsConnected())
                        writer.WriteAttributeString("value", port.GetValue().ToString());
                    else
                        writer.WriteAttributeString("connected", port.ConnectedTo[0].Parent.Name + "+" + port.ConnectedTo[0].Name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Deserialize object
        /// </summary>
        public void Load(Stream stream, NodeTypesManager _nodeTypes)
        {
            this.Connections.Clear();
            this.Nodes.Clear();

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            Scheme = new Main.Sceme.Scheme(doc.DocumentElement.Attributes["name"].Value);
            List<Tuple<string, string>> _connectionsToCreate = new List<Tuple<string, string>>();

            // First create all nodes
            foreach (XmlNode t in doc.DocumentElement.ChildNodes)
            {
                string nodeName = t.Attributes["name"].Value;
                string nodeType = t.Attributes["nodeType"].Value;
                string x = t.Attributes["x"].Value;
                string y = t.Attributes["y"].Value;
                var node = CreateNode(NodeTypes.GetNodeType(nodeType), Convert.ToDouble(x), Convert.ToDouble(y), false, nodeName);
                foreach (XmlNode z in t.ChildNodes)
                {
                    string portName = z.Name;
                    string value = z.Attributes["value"] == null ? null : z.Attributes["value"].Value;
                    string connected = z.Attributes["connected"] == null ? null : z.Attributes["connected"].Value;
                    if (connected != null)
                        _connectionsToCreate.Add(new Tuple<string, string>(nodeName + "+" + portName, connected));
                    else
                    {
                        // We can set const value here
                        InputPortViewModel port = node.InputPorts.FirstOrDefault((u) => u.PortName == portName);
                        if (port.Port.DataType == typeof(bool))
                        {
                            port.Val = Convert.ToBoolean(value);
                            continue;
                        }
                        if (port.Port.DataType == typeof(int))
                        {
                            port.Val = Convert.ToInt32(value);
                            continue;
                        }
                        throw new ArgumentException(string.Format("{0} can't be desirialized", value.GetType()));
                    }
                }
            }

            // Create connections (all nodes are created before)
            foreach (Tuple<string, string> conn in _connectionsToCreate)
            {
                string firstPortNode = conn.Item1.Split('+')[0];
                string firstPort = conn.Item1.Split('+')[1];
                string secondPortNode = conn.Item2.Split('+')[0];
                string secondPort = conn.Item2.Split('+')[1];

                PortViewModel first = Nodes.FirstOrDefault((r) => r.Name == firstPortNode).InputPorts.FirstOrDefault((u) => u.PortName == firstPort);
                if (first == null)
                    first = Nodes.FirstOrDefault((r) => r.Name == firstPortNode).OutputPorts.FirstOrDefault((u) => u.PortName == firstPort);
                PortViewModel second = Nodes.FirstOrDefault((r) => r.Name == secondPortNode).InputPorts.FirstOrDefault((u) => u.PortName == secondPort);
                if (second == null)
                    second = Nodes.FirstOrDefault((r) => r.Name == secondPortNode).OutputPorts.FirstOrDefault((u) => u.PortName == secondPort);

                CreateConnection(first, second);
            }
        }

        #region Dropable
        public System.Type DataType
        {
            get { return typeof(TypeViewModel); }
        }

        public void Drop(DragEventArgs e)
        {
            // Get mouse position relative to canvas in WorkArea
            Point pos = e.GetPosition(e.OriginalSource as IInputElement);
            CreateNode((e.Data.GetData(typeof(TypeViewModel)) as TypeViewModel).NodeType, pos.X, pos.Y, true);
        }
        #endregion
    }
}

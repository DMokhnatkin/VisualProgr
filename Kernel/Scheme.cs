using Main.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;

namespace Main.Sceme
{
    /// <summary>
    /// Scene contains nodes, connections, values
    /// </summary>
    public class Scheme
    {
        #region Events
        public delegate void ConnectionChangedHandler(Main.Ports.Port port1, Main.Ports.Port port2);

        /// <summary>
        /// Raises when new connection created
        /// </summary>
        public event ConnectionChangedHandler ConnectionCreated;

        /// <summary>
        /// Raises when connecton removed
        /// </summary>
        public event ConnectionChangedHandler ConnectionRemoved;

        public delegate void NodeChangedHandler(Main.Nodes.Node sender);

        /// <summary>
        /// Raises when new node created
        /// </summary>
        public event NodeChangedHandler NodeCreated;
        

        #endregion

        public string Name { get; private set; }

        /// <summary>
        /// Store all nodes
        /// </summary>
        
        Main.Nodes.NodeManager NodesManager { get; set; }

        /// <summary>
        /// Node types available in scheme
        /// </summary>
        public NodeTypesManager NodeTypes { get; private set; }

        /// <summary>
        /// Get all nodes in scheme
        /// </summary>
        public List<Node> Nodes { get { return NodesManager.GetAllNodes(); } }

        public Scheme(string name)
        {
            Name = name;
            NodesManager = new Main.Nodes.NodeManager();
        }

        /// <summary>
        /// Create new node
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="name"></param>
        public Node CreateNode(Main.Nodes.NodeType nodeType, string name=null)
        {
            var newNode = NodesManager.CreateNode(nodeType, name);
            if (NodeCreated != null)
                NodeCreated(newNode);
            return newNode;
        }

        /// <summary>
        /// Connect ports
        /// </summary>
        /// <param name="port1"></param>
        /// <param name="port2"></param>
        public void Connect(Main.Ports.Port port1, Main.Ports.Port port2)
        {
            port1.Parent.ConnectionManager.CreateConnection(port1, port2);
            if (ConnectionCreated != null)
                ConnectionCreated(port1, port2);
        }
        
        /// <summary>
        /// Clear scene
        /// </summary>
        public void Clear()
        {
            NodesManager.Clear();
        }
    }
}

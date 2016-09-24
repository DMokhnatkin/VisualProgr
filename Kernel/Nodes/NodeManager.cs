using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace Main.Nodes
{
    /// <summary>
    /// Uses to store nodes
    /// </summary>
    
    internal class NodeManager
    {
        /// <summary>
        /// Nodes storage. Name - Node
        /// </summary>
        
        Dictionary<string, Node> _nodes = new Dictionary<string, Node>();

        /// <summary>
        /// Last used name for node
        /// </summary>
        
        string _lastName = "node1";

        internal NodeManager()
        { }

        /// <summary>
        /// Get free name for node
        /// </summary>
        string FreeName
        {
            get
            {
                // If _freeName is not free we will find other
                while (_nodes.ContainsKey(_lastName))
                {
                    Regex reg = new Regex(@"([^0-9]*)([0-9]+)");
                    Match match = reg.Match(_lastName);
                    _lastName = match.Groups[1].Value + (Convert.ToInt32(match.Groups[2].Value) + 1).ToString();
                }
                return _lastName;
            }
        }

        /// <summary>
        /// Create new node
        /// </summary>
        /// <param name="nodeType">Type of node class</param>
        public Node CreateNode(NodeType nodeType, string name = null)
        {
            if (name == null)
                name = FreeName;
            else
                if (_nodes.ContainsKey(name))
                    throw new ArgumentException(string.Format("Node '{0}' has been created before", name));
            Node newNode = (Node)Activator.CreateInstance(nodeType.SystemType, name, nodeType);
            // Register new node
            _nodes.Add(name, newNode);
            return newNode;
        }

        /// <summary>
        /// Get node by name
        /// </summary>
        /// <param name="name">Node's name </param>
        public Node GetNode(string name)
        {
            if (!_nodes.ContainsKey(name))
                throw new ArgumentException("There is no node with name " + name);
            return _nodes[name];
        }

        /// <summary>
        /// Get all nodes
        /// </summary>
        /// <returns></returns>
        public List<Node> GetAllNodes()
        {
            return _nodes.Values.ToList();
        }

        /// <summary>
        /// Delete all nodes 
        /// </summary>
        public void Clear()
        {
            _nodes.Clear();
        }
    }
}

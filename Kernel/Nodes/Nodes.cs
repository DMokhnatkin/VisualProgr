using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Main.Nodes
{
    
    public abstract class Node
    {
        /// <summary>
        /// To store and create ports
        /// </summary>
        
        public Main.Ports.PortManager PortManager { get; set; }

        /// <summary>
        /// To store and create connections
        /// </summary>
        //
        internal Main.Connections.ConnectionManager ConnectionManager { get; private set; }

        /// <summary>
        /// Name of Node
        /// </summary>
        
        public string Name { get; private set; }

        /// <summary>
        /// Node type of this node.
        /// </summary>
        public NodeType NodeType { get; private set; }

        /// <summary>
        /// Name of node type(you can use hierarchy)
        /// /standart/boolean/and
        /// </summary>
        public static readonly string typeName = "/basic/basic";

        /// <summary>
        /// If all port values in this node are up to date
        /// </summary>
        public bool UpToDate { get; internal set; }

        /// <summary>
        /// We can create Node only from same assembly
        /// </summary>
        /// <param name="name"></param>
        protected internal Node(string name, NodeType _nodeType)
        {
            Name = name;
            PortManager = new Main.Ports.PortManager(this);
            ConnectionManager = new Main.Connections.ConnectionManager();
            NodeType = _nodeType;
        }

        public override string ToString()
        {
            return string.Format("Type = '{0}' ; Name = '{1}'", this.GetType().GetField("typeName").GetValue(null).ToString(), this.Name);
        }

        /// <summary>
        /// Calculate values and return List of results on output ports
        /// First element in tuple is output port name
        /// Second element un tuple is output port value
        /// </summary>
        public abstract List<Tuple<string,object>> Calculate();
    }
}

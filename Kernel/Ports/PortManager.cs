using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Main.Ports
{
    /// <summary>
    /// Is used to create ports and store them
    /// </summary>
    
    public class PortManager
    {
        /// <summary>
        /// Parent node of this PortManager
        /// </summary>
        Main.Nodes.Node _parent;

        /// <summary>
        /// We will store ports in dictionary (key - portName, val - Port)
        /// </summary>
        
        Dictionary<string, Port> _ports = new Dictionary<string, Port>();

        internal PortManager(Main.Nodes.Node parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Create new port
        /// </summary>
        /// <param name="port">Port to create</param>
        public void CreatePort(Port port)
        {
            if (_ports.ContainsKey(port.Name))
                throw new ArgumentException(string.Format("Port with name '{0}' in node '{1}' is exists", port.Name, _parent.Name));
            port.Parent = _parent;
            _ports.Add(port.Name, port);
        }

        /// <summary>
        /// Get all ports
        /// </summary>
        /// <returns>List of ports or empty list</returns>
        public List<Port> GetPorts()
        {
            return _ports.Values.ToList();
        }

        /// <summary>
        /// Get all ports of type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of ports or empty list</returns>
        public List<Port> GetPorts(PortType type)
        {
            return (from port in _ports.Values where port.PortType == type select port).ToList();
        }

        /// <summary>
        /// Get port by name
        /// </summary>
        public Port GetPort(string portName)
        {
            if (!_ports.ContainsKey(portName))
                throw new ArgumentException(string.Format("There is no port '{0}' in '{1} node", portName, _parent.Name));
            return _ports[portName];
        }
    }
}

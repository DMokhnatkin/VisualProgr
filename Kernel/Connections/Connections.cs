using System;
using Main.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Main.Connections
{
    public class ConnectionManager
    {

        // Storage for connections
        
        Dictionary<Port, List<Port>> _connections = new Dictionary<Port, List<Port>>();
        
        internal ConnectionManager()
        { }

        /// <summary>
        /// Is port connected to some other port or ports
        /// </summary>
        public static bool Connected(Port port)
        {
            Main.Connections.ConnectionManager _curConnManager = port.Parent.ConnectionManager;
            if (!_curConnManager._connections.ContainsKey(port))
                return false;
            return _curConnManager._connections[port].Count != 0;
        }

        /// <summary>
        /// Return if find is connected with cur(maybe recursively)
        /// </summary>
        public static void LoopVerify(Main.Nodes.Node find, Main.Nodes.Node cur, ref bool loop)
        {
            if (find == cur)
            {
                // Initialy the same node was given
                loop = true;
                return;
            }
            var outputs = cur.PortManager.GetPorts(PortType.Output);
            foreach (Port t in outputs)
            {
                if (loop)
                    return;
                if (!Connected(t))
                    continue;
                foreach (Port z in GetAllConnections(t))
                {
                    if (z.Parent == find)
                    {
                        loop = true;
                        return;
                    }
                    else
                        LoopVerify(find, z.Parent, ref loop);
                }
            }
        }

        /// <summary>
        /// Can ports be connected or not.
        /// If port can't be created exception will be thrown.
        /// </summary>
        public static void CanBeConnected(Port port1, Port port2)
        {
            // Verify port types (input, ouput, ..)
            if (port1.PortType == port2.PortType)
                throw new PortTypesException(port1, port2);
            // Verify data types (bool, int, ..)
            if (port1.DataType != port2.DataType)
                throw new DataTypeException(port1, port2);
            // Lets make port1 - output and port2 - input
            if (port1.PortType == PortType.Input)
            {
                Port r = port1;
                port1 = port2;
                port2 = r;
            }
            // If input port is connected throw exception
            if (port2.IsConnected())
                throw new PortIsConnectedException(port2);
            // Verify if there will be loop after connect
            bool loop = false;
            LoopVerify(port1.Parent, port2.Parent, ref loop);
            if (loop)
                throw new InfiniteLoopException(port1, port2);
        }

        /// <summary>
        /// Create new connection
        /// </summary>
        public void CreateConnection(Port port1, Port port2)
        {
            CanBeConnected(port1, port2);
            if (port1.PortType == PortType.Input)
            {
                Port r = port1;
                port1 = port2;
                port2 = r;
            }
            if (!_connections.ContainsKey(port1))
                _connections.Add(port1, new List<Port>());
            if (!port2.Parent.ConnectionManager._connections.ContainsKey(port2))
                port2.Parent.ConnectionManager._connections.Add(port2, new List<Port>());
            _connections[port1].Add(port2);
            port2.Parent.ConnectionManager._connections[port2].Add(port1);
            Main.Values.ValueManager.DelValue(port2); // Del value on input
            port1.OnConnectionChanged();
            port2.OnConnectionChanged();
        }

        /// <summary>
        /// Remove connection between two ports
        /// </summary>
        public static void BreakConnection(Port port1, Port port2)
        {
            if (port1.Parent.ConnectionManager._connections.ContainsKey(port1) && port2.Parent.ConnectionManager._connections.ContainsKey(port2) &&
                port1.Parent.ConnectionManager._connections[port1].Contains(port2) && port2.Parent.ConnectionManager._connections[port2].Contains(port1))
            {
                port1.Parent.ConnectionManager._connections[port1].Remove(port2);
                port2.Parent.ConnectionManager._connections[port2].Remove(port1);
                port1.OnConnectionChanged();
                port2.OnConnectionChanged();
            }
            else
                throw new ArgumentException(string.Format("Conenction between '{0}' and '{1}' is not exists"));
        }

        /// <summary>
        /// Remove connections
        /// </summary>
        public static void BreakConnection(Port port1)
        {
            foreach (Port t in GetAllConnections(port1))
                BreakConnection(port1, t);
            port1.OnConnectionChanged();
        }

        /// <summary>
        /// Return all connected ports
        /// </summary>
        public static List<Port> GetAllConnections(Port port)
        {
            Main.Connections.ConnectionManager _curConnManager = port.Parent.ConnectionManager;
            if (_curConnManager._connections.ContainsKey(port))
                return _curConnManager._connections[port];
            else
                return new List<Port>();
        }
    }
}

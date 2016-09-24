using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Connections
{
    /// <summary>
    /// Exception throws when user try connect input port which is already connected
    /// </summary>
    public class PortIsConnectedException : Exception
    {
        public Main.Ports.Port Port { get; private set; }

        public PortIsConnectedException(Main.Ports.Port port)
            : base(string.Format("{0} is already connected", port))
        {
            Port = port;
        }
    }

    /// <summary>
    /// Exception throws when user try connect ports and it will cause loop
    /// </summary>
    public class InfiniteLoopException : Exception
    {
        public Main.Ports.Port Port1 { get; private set; }
        public Main.Ports.Port Port2 { get; private set; }

        public InfiniteLoopException(Main.Ports.Port port1, Main.Ports.Port port2) 
            : base(string.Format("Connected {0} and {1} will cause loop", port1, port2))
        {
            Port1 = port1;
            Port2 = port2;
        }
    }

    /// <summary>
    /// Exception throws when user try connect ports with incompatible types (f.e. two input ports)
    /// </summary>
    public class PortTypesException : Exception
    {
        public Main.Ports.Port Port1 { get; private set; }
        public Main.Ports.Port Port2 { get; private set; }

        public PortTypesException(Main.Ports.Port port1, Main.Ports.Port port2)
            : base(string.Format("Types of port {0} and {1} are incompatible", port1, port2))
        {
            Port1 = port1;
            Port2 = port2;
        }
    }

    /// <summary>
    /// Exception throws when user try connect ports with incompatible data types (f.e. bool with int)
    /// </summary>
    public class DataTypeException : Exception
    {
        public Main.Ports.Port Port1 { get; private set; }
        public Main.Ports.Port Port2 { get; private set; }

        public DataTypeException(Main.Ports.Port port1, Main.Ports.Port port2) 
            : base(string.Format("To connect {0} and {1} their data types must be the same", port1, port2))
        {
            Port1 = port1;
            Port2 = port2;
        }
    }
}

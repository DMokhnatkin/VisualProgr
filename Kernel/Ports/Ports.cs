using System;
using Main.Nodes;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Main.Ports
{
    public enum PortType { Input = 1, Output = 2}

    /// <summary>
    /// All ports must implement this interface
    /// </summary>
    public class Port
    {
        /// <summary>
        /// Name of this port
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Parent(Node) of this port
        /// </summary>
        public Node Parent { get; protected internal set; }

        /// <summary>
        /// Type of this port(Input, Output, ...)
        /// </summary>
        private PortType _portType;
        public PortType PortType { get {return _portType;} protected set { _portType = value; } }

        /// <summary>
        /// Get type of data which can be in this port
        /// </summary>
        public Type DataType { get; private set; }

        /// <summary>
        /// It stores value
        /// </summary>
        
        public Main.Values.ValueManager ValueManager { get; private set; }

        /// <summary>
        /// Get value in port
        /// </summary>
        public object GetValue()
        {
            return Main.Values.ValueManager.GetVal(this);
        }

        /// <summary>
        /// Set value in port
        /// </summary>
        public void SetValue(object val)
        {
            if (val.GetType() != DataType)
                throw new FormatException(string.Format("'{0}' can't contain {1}", this, val.GetType()));
            Main.Values.ValueManager.SetValue(this, val);
        }

        /// <summary>
        /// Get if this port in connected
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return Main.Connections.ConnectionManager.Connected(this);
        }

        /// <summary>
        /// Get all port which connected with current
        /// </summary>
        public List<Port> ConnectedTo { get { return Main.Connections.ConnectionManager.GetAllConnections(this); } }

        public Port(string name, PortType portType, Type dataType)
        {
            Name = name;
            PortType = portType;
            DataType = dataType;
            ValueManager = new Main.Values.ValueManager(this);
        }

        public override string ToString()
        {
            return string.Format("[Name='{0}'; Node='{1}'; Type='{2}'; DataType='{3}']", Name, Parent.Name, PortType, DataType);
        }

        #region Events
        /// <summary>
        /// Raies when connection in this port changed
        /// </summary>
        public event EventHandler ConnectionChanged = (x, y) => { };

        internal void OnConnectionChanged()
        {
            ConnectionChanged(this, new EventArgs());
        }

        #endregion
    }
}

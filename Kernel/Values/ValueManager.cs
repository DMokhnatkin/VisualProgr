using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Main.Values
{
    public class ValueManager
    {
        /// <summary>
        /// Port which contains this value manager
        /// </summary>
        public Main.Ports.Port ParentPort { get; private set; }

        /// <summary>
        /// Value in this port
        /// </summary>
        
        private object Value { get; set; }

        private bool _upToDate = true;
        /// <summary>
        /// If true, this port's value is out of date
        /// </summary>
        public bool UpToDate 
        {
            get { return _upToDate;}
            private set 
            { 
                _upToDate = value; 
                OnUpToDateChanged();
            } 
        }

        /// <summary>
        /// Delete value if exists
        /// </summary>
        public static void DelValue(Main.Ports.Port port)
        {
            PaintOutputs(port.Parent, false, true);
            port.ValueManager.Value = null;
        }

        /// <summary>
        /// Set UpToDate = flag to single port
        /// </summary>
        /// <param name="port"></param>
        /// <param name="recursively">Set recursively to all connected</param>
        private static void PaintPort(Main.Ports.Port port, bool flag, bool recursively)
        {
            port.ValueManager.UpToDate = flag;
            if (recursively)
                foreach (Main.Ports.Port connected in Main.Connections.ConnectionManager.GetAllConnections(port))
                    PaintOutputs(connected.Parent, flag, true);
        }


        /// <summary>
        /// Set update = flag to all outputs of this node and do it for all conencted
        /// </summary>
        private static void PaintOutputs(Main.Nodes.Node node, bool flag, bool recursively)
        {
            foreach (Main.Ports.Port output in node.PortManager.GetPorts(Main.Ports.PortType.Output))
                PaintPort(output, flag, recursively);
        }

        /// <summary>
        /// Set value for input
        /// </summary>
        /// <param name="port"></param>
        /// <param name="val"></param>
        public static void SetValue(Main.Ports.Port port, object newVal)
        {
            switch (port.PortType)
            {
                case Main.Ports.PortType.Input:
                    port.ValueManager.Value = newVal;
                    port.ValueManager.UpToDate = true;
                    PaintOutputs(port.Parent, false, true);
                    break;
                case Main.Ports.PortType.Output:
                    port.ValueManager.Value = newVal;
                    PaintPort(port, false, true);
                    port.ValueManager.UpToDate = true;
                    break;
            }
        }

        /// <summary>
        /// Get value in port
        /// </summary>
        public static object GetVal(Main.Ports.Port port)
        {
            if (port.PortType == Main.Ports.PortType.Input)
                if (Main.Connections.ConnectionManager.GetAllConnections(port).Count == 0)
                    // If port is not connected we will return value in this input (Value property)
                    return port.ValueManager.Value;
                else
                    // If port is connected we will calculate and return value in connected output
                    return GetVal(Main.Connections.ConnectionManager.GetAllConnections(port)[0]);

            // If we don't need to update value, just return it
            if (port.ValueManager.UpToDate)
                return port.ValueManager.Value;
            else
                // If update = true we must calculate value. For calculation we need input ports, so let's update them
                foreach(Main.Ports.Port input in port.Parent.PortManager.GetPorts(Main.Ports.PortType.Input))
                {
                    GetVal(input);
                }
            // Set calculated values
            foreach (Tuple<string, object> outp_val in port.Parent.Calculate())
            {
                SetValue(port.Parent.PortManager.GetPort(outp_val.Item1), outp_val.Item2);
            }
            // Now all output ports have right values. (We calculated them) Let's set UpToDate = true to them
            PaintOutputs(port.Parent, true, false);
            return port.ValueManager.Value;
        }

        public ValueManager(Main.Ports.Port parentPort)
        {
            Value = null;
            UpToDate = true;
            ParentPort = parentPort;
        }

        #region Events
        /// <summary>
        /// Raised when up to date property changed
        /// Sender is port
        /// </summary>
        EventHandler upToDateChanged = (x, y) => { };

        public event EventHandler UpToDateChanged
        {
            add { upToDateChanged += value; }
            remove { upToDateChanged -= value; }
        }

        protected void OnUpToDateChanged()
        {
            upToDateChanged(this.ParentPort, new EventArgs());
        }
        #endregion
    }
}

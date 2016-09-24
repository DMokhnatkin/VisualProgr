using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VisualProgrGUI.ViewModel.Commands;

namespace VisualProgrGUI.ViewModel
{

    public abstract class PortViewModel : DependencyObject
    {
        #region Dependency properties

        public static readonly DependencyProperty PortNameProperty =
            DependencyProperty.Register("PortName", typeof(string), typeof(PortViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty IsNotConenctedProperty =
            DependencyProperty.Register("IsNotConnected", typeof(bool), typeof(PortViewModel), new PropertyMetadata(false));

        public static readonly DependencyProperty HotspotProperty =
            DependencyProperty.Register("Hotspot", typeof(Point), typeof(PortViewModel), new PropertyMetadata(new Point()));

        #endregion

        /// <summary>
        /// Port model
        /// </summary>
        public Main.Ports.Port Port { get; private set; }

        /// <summary>
        /// Node which contains this ports
        /// </summary>
        public NodeViewModel Parent { get; private set; }

        /// <summary>
        /// Name of this port
        /// </summary>
        public string PortName
        {
            get { return (string)GetValue(PortNameProperty); }
            set { SetValue(PortNameProperty, value); }
        }

        /// <summary>
        /// Is this port conencted
        /// </summary>
        public bool IsNotConnected
        {
            get { return (bool)GetValue(IsNotConenctedProperty); }
            set { SetValue(IsNotConenctedProperty, value); }
        }

        /// <summary>
        /// Hotspot of this port
        /// </summary>
        public Point Hotspot
        {
            get { return (Point)GetValue(HotspotProperty); }
            set { SetValue(HotspotProperty, value); }
        }

        public PortViewModel(Main.Ports.Port _model, NodeViewModel parent)
        {
            Parent = parent;
            Port = _model;
            PortName = Port.Name;
            IsNotConnected = true;
            Port.ConnectionChanged += Port_ConnectionChanged;
        }

        #region Private members
        /// <summary>
        /// Raised when connection in this port changed by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Port_ConnectionChanged(object sender, EventArgs e)
        {
            IsNotConnected = !Port.IsConnected();
        }

        #endregion
    }
}

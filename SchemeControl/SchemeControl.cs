using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchemeControlLib
{
    /// <summary>
    /// Args for connection created event
    /// </summary>
    public class ConnectionCreatedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Connection can be created
        /// </summary>
        public bool CanBeCreated { get; set; }

        public PortControl First { get; private set; }

        public PortControl Second { get; private set; }

        public ConnectionCreatedEventArgs(RoutedEvent routedEvent, object source, PortControl first, PortControl second) : base(routedEvent, source)
        {
            CanBeCreated = true;
            First = first;
            Second = second;
        }
    }

    /// <summary>
    /// Delegate for connection created event handlers
    /// </summary>
    public delegate void ConnectionCreatedEventHandler(object sender, ConnectionCreatedEventArgs e);

    /// <summary>
    /// Control to present scheme
    /// Contains nodes, conenctions
    /// </summary>
    [TemplatePart(Name = "PART_SchemeItems", Type=typeof(NodesControl))]
    [TemplatePart(Name = "PART_Canvas", Type=typeof(Canvas))]
    [TemplatePart(Name = "PART_Connections", Type=typeof(ItemsControl))]
    public class SchemeControl : Selector
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty NodeTemplateProperty =
            DependencyProperty.Register("NodeTemplate", typeof(DataTemplate), typeof(SchemeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty NodeContainerStyleProperty =
            DependencyProperty.Register("NodeContainerStyle", typeof(Style), typeof(SchemeControl), new PropertyMetadata(null));      

        internal static readonly RoutedEvent ConnectionCreatedEvent = EventManager.RegisterRoutedEvent("ConnectionCreated",
            RoutingStrategy.Bubble, typeof(ConnectionCreatedEventHandler), typeof(PortControl));

        public static readonly DependencyProperty ConnectionsProperty =
            DependencyProperty.Register("Connections", typeof(ObservableCollection<object>), typeof(SchemeControl), new PropertyMetadata(null));
        
        public static RoutedEvent PortSelectedEvent = PortControl.PortSelectedEvent.AddOwner(typeof(SchemeControl));

        public static readonly DependencyProperty CurConnectionProperty =
            DependencyProperty.Register("CurConnection", typeof(ConnectionControl), typeof(SchemeControl), new PropertyMetadata(null));

        #endregion

        static SchemeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SchemeControl), new FrameworkPropertyMetadata(typeof(SchemeControl)));
        }

        /// <summary>
        /// Data template for node
        /// </summary>
        public DataTemplate NodeTemplate
        {
            get { return (DataTemplate)GetValue(NodeTemplateProperty); }
            set { SetValue(NodeTemplateProperty, value); }
        }

        /// <summary>
        /// Event which raises when port selected
        /// </summary>
        public event RoutedEventHandler PortSelected
        {
            add { base.AddHandler(PortControl.PortSelectedEvent, value); }
            remove { base.RemoveHandler(PortControl.PortSelectedEvent, value); }
        }

        /// <summary>
        /// Event which raises when conenction created
        /// </summary>
        public event ConnectionCreatedEventHandler ConnectionCreated
        {
            add { base.AddHandler(ConnectionCreatedEvent, value); }
            remove { base.RemoveHandler(ConnectionCreatedEvent, value); }
        }

        /// <summary>
        /// Style of node container
        /// </summary>
        public Style NodeContainerStyle
        {
            get { return (Style)GetValue(NodeContainerStyleProperty); }
            set { SetValue(NodeContainerStyleProperty, value); }
        }

        /// <summary>
        /// Conenctions in scheme
        /// </summary>
        public ObservableCollection<object> Connections
        {
            get { return (ObservableCollection<object>)GetValue(ConnectionsProperty); }
            set { SetValue(ConnectionsProperty, value); }
        }

        /// <summary>
        /// Conenction which is creates now
        /// </summary>
        public ConnectionControl CurConnection
        {
            get { return (ConnectionControl)GetValue(CurConnectionProperty); }
            set { SetValue(CurConnectionProperty, value); }
        }

        public SchemeControl()
        {
            this.Loaded += SchemeControl_Loaded;
            this.ConnectionCreated += (x, y) => { };          
        }

        #region Private Methods

        // First selected port. We use it to create connection
        private PortControl _selectedPort;

        // Nodes
        private NodesControl _schemeItems;

        // Canvas
        private Canvas _canvas;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _schemeItems = GetTemplateChild("PART_SchemeItems") as NodesControl;
            _canvas = GetTemplateChild("PART_Canvas") as Canvas;
        }

        /// <summary>
        /// Raised when scheme control loaded
        /// </summary>
        void SchemeControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.PortSelected += SchemeControl_PortSelected;
            Connections = new ObservableCollection<object>();
        }

        /// <summary>
        /// Start create new conenction
        /// </summary>
        /// <param name="port"></param>
        private void StartConnectionCreation(PortControl port)
        {
            // If there is no selected port, we just set current as selected
            _selectedPort = port;
            _selectedPort.IsConnected = true;
            CurConnection = new ConnectionControl(_selectedPort, this);
        }

        /// <summary>
        /// End connection creation
        /// </summary>
        private void EndConnectionCreation()
        {
            PortControl.UnSelectCommand.Execute(null, _selectedPort);
            CurConnection.Dispose();
            Connections.Remove(CurConnection);
            CurConnection = null;
            _selectedPort = null;
        }

        /// <summary>
        /// Raised when some child port selected
        /// </summary>
        void SchemeControl_PortSelected(object sender, RoutedEventArgs e)
        {
            if (_selectedPort == null)
                StartConnectionCreation(e.OriginalSource as PortControl);
            else
            {
                if (!CurConnection.First.IsLoaded)
                {
                    CurConnection = null;
                    return;
                }
                // Connection created event args will contains CanBeCreated property
                ConnectionCreatedEventArgs r;
                OnConnectionCreated(out r, _selectedPort, e.OriginalSource as PortControl);
                if (r.CanBeCreated)
                {
                    (e.OriginalSource as PortControl).IsConnected = true;

                    PortControl.UnSelectCommand.Execute(null, e.OriginalSource as PortControl);

                    EndConnectionCreation();
                }
            }
        }

        /// <summary>
        /// Select to Raise connection created event
        /// </summary>
        protected virtual void OnConnectionCreated(out ConnectionCreatedEventArgs e, PortControl first, PortControl second)
        {
            e = new ConnectionCreatedEventArgs(ConnectionCreatedEvent, this, first, second);
            base.RaiseEvent(e);
        }

        #endregion
    }
}

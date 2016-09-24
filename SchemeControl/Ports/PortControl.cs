using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Port control
    /// </summary>
    [TemplatePart(Name="PART_Hotspot", Type=typeof(FrameworkElement))]
    public class PortControl : ContentControl
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty HotspotProperty =
            DependencyProperty.Register("Hotspot", typeof(Point), typeof(PortControl), new PropertyMetadata(new Point(0,0)));    

        public static readonly DependencyProperty ParentSchemeControlProperty = SchemeItem.ParentSchemeControlProperty.AddOwner(typeof(PortControl));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(PortControl), new PropertyMetadata(false, new PropertyChangedCallback(IsSelectedPropertyChanged)));

        public static readonly DependencyProperty IsConnectedProperty =
            DependencyProperty.Register("IsConnected", typeof(bool), typeof(PortControl), new PropertyMetadata(false));

        public static readonly DependencyProperty HotspotStyleProperty =
            DependencyProperty.Register("HotspotStyle", typeof(Style), typeof(PortControl), new PropertyMetadata(null));

        public static readonly DependencyProperty HotspotTemplateProperty =
            DependencyProperty.Register("HotspotTemplate", typeof(ControlTemplate), typeof(PortControl), new PropertyMetadata(null));

        // Events
        internal static readonly RoutedEvent PortSelectedEvent = EventManager.RegisterRoutedEvent("PortSelected",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PortControl));

        // Commands
        public static RoutedCommand SelectCommand = new RoutedCommand("Select", typeof(PortControl));

        public static RoutedCommand UnSelectCommand = new RoutedCommand("UnSelect", typeof(PortControl));

        #endregion

        /// <summary>
        /// Event which raises when port selected
        /// </summary>
        public event RoutedEventHandler PortSelected
        {
            add { base.AddHandler(PortControl.PortSelectedEvent, value); }
            remove { base.RemoveHandler(PortControl.PortSelectedEvent, value); }
        }

        /// <summary>
        /// If this port is selected
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Scheme control which contains this port
        /// </summary>
        public SchemeControl ParentSchemeControl
        {
            get { return (SchemeControl)GetValue(ParentSchemeControlProperty); }
            set { SetValue(ParentSchemeControlProperty, value); }
        }

        /// <summary>
        /// Point to attach arrow
        /// </summary>
        public Point Hotspot
        {
            get { return (Point)GetValue(HotspotProperty); }
            set { SetValue(HotspotProperty, value); }
        }

        /// <summary>
        /// If this port is connected
        /// </summary>
        public bool IsConnected
        {
            get { return (bool)GetValue(IsConnectedProperty); }
            set { SetValue(IsConnectedProperty, value); }
        }

        /// <summary>
        /// Style for hotspot
        /// </summary>
        public Style HotspotStyle
        {
            get { return (Style)GetValue(HotspotStyleProperty); }
            set { SetValue(HotspotStyleProperty, value); }
        }

        /// <summary>
        /// Template for hotspot
        /// </summary>
        public ControlTemplate HotspotTemplate
        {
            get { return (ControlTemplate)GetValue(HotspotTemplateProperty); }
            set { SetValue(HotspotTemplateProperty, value); }
        }

        public PortControl()
        {
            this.LayoutUpdated += PortControl_LayoutUpdated;
            this.IsConnected = false;
        }

        #region Private Methods

        /// <summary>
        /// Used to connect ports
        /// </summary>
        FrameworkElement _hotspotElement;

        static PortControl()
        {
            CommandBinding binding = new CommandBinding();
            binding.Command = SelectCommand;
            binding.Executed += new ExecutedRoutedEventHandler(ExecutedSelectCommand);
            CommandManager.RegisterClassCommandBinding(typeof(PortControl), binding);

            binding = new CommandBinding();
            binding.Command = UnSelectCommand;
            binding.Executed += new ExecutedRoutedEventHandler(ExecutedUnSelectCommand);
            CommandManager.RegisterClassCommandBinding(typeof(PortControl), binding);
        }

        /// <summary>
        /// Raised when layout updated
        /// </summary
        void PortControl_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateHotspot();
        }

        /// <summary>
        /// Calculate hotspot
        /// </summary>
        void UpdateHotspot()
        {
            if (this.ParentSchemeControl == null)
                return;
            if (_hotspotElement == null)
                return;
            Point newPoint = _hotspotElement.TranslatePoint(new Point(_hotspotElement.ActualWidth / 2.0d, _hotspotElement.ActualHeight / 2.0d), this.ParentSchemeControl);
            if (this.Hotspot != newPoint)
                this.Hotspot = newPoint;
        }

        /// <summary>
        /// Raise PortSelectedEvent
        /// </summary>
        private static void IsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RoutedEventArgs t = new RoutedEventArgs(PortControl.PortSelectedEvent, sender);
                (sender as PortControl).RaiseEvent(t);
            }
        }

        /// <summary>
        /// Executed select command
        /// </summary>
        private static void ExecutedSelectCommand(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as PortControl).IsSelected = true;
        }

        /// <summary>
        /// Executed unselect command
        /// </summary>
        private static void ExecutedUnSelectCommand(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as PortControl).IsSelected = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _hotspotElement = GetTemplateChild("PART_Hotspot") as FrameworkElement;
            ParentSchemeControl = FindAncestor.FindAncestorOfType(this, typeof(SchemeControl)) as SchemeControl;
        }
        #endregion

    }
}

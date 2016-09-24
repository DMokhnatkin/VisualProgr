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
    /// This control used to present connection which is creating (between port and mouse position)
    /// </summary>
    public class ConnectionControl : Control, IDisposable
    {
        #region Dependency properties / Events

        public static readonly DependencyProperty Hotspot1Property =
            DependencyProperty.Register("Hotspot1", typeof(Point), typeof(ConnectionControl), new PropertyMetadata(new Point()));

        public static readonly DependencyProperty Hotspot2Property =
            DependencyProperty.Register("Hotspot2", typeof(Point), typeof(ConnectionControl), new PropertyMetadata(new Point()));
        
        #endregion

        /// <summary>
        /// Connected port
        /// </summary>
        public PortControl First
        {
            get { return _first; }
            private set 
            {
                if (value != null)
                {
                    Binding binding = new Binding("Hotspot");
                    binding.Source = value;
                    BindingOperations.SetBinding(this, Hotspot1Property, binding);
                }
                _first = value;
            }
        }

        /// <summary>
        /// Create connection between port and cursor position
        /// </summary>
        /// <param name="first"></param>
        /// <param name="element">Element which mouse move event will be handled</param>
        public ConnectionControl(PortControl first, FrameworkElement element)
        {
            First = first;

            _mouseMoveElement = element;   

            _mouseMoveElement.MouseMove += element_MouseMove;

            Hotspot2 = Hotspot1;
        }

        /// <summary>
        /// Calculate Hotspot 2
        /// </summary>
        void element_MouseMove(object sender, MouseEventArgs e)
        {
            Hotspot2 = e.GetPosition(_mouseMoveElement);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _mouseMoveElement.MouseMove -= element_MouseMove;
        }

        /// <summary>
        /// First hotspot point
        /// </summary>
        public Point Hotspot1
        {
            get { return (Point)GetValue(Hotspot1Property); }
            set { SetValue(Hotspot1Property, value); }
        }

        /// <summary>
        /// Second hotspot
        /// </summary>
        public Point Hotspot2
        {
            get { return (Point)GetValue(Hotspot2Property); }
            set { SetValue(Hotspot2Property, value); }
        }

        #region Private members

        // Element which is under cursor
        UIElement _mouseMoveElement = null;

        PortControl _first;

        static ConnectionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConnectionControl), new FrameworkPropertyMetadata(typeof(ConnectionControl)));
        }

        #endregion
    }
}

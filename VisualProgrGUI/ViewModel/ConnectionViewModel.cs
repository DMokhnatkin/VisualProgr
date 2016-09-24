using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrGUI.ViewModel
{
    public class ConnectionViewModel : DependencyObject
    {
        #region Dependency properties / Events

        public static readonly DependencyProperty Hotspot1Property =
            DependencyProperty.Register("Hotspot1", typeof(Point), typeof(ConnectionViewModel), new PropertyMetadata(new Point()));

        public static readonly DependencyProperty Hotspot2Property =
            DependencyProperty.Register("Hotspot2", typeof(Point), typeof(ConnectionViewModel), new PropertyMetadata(new Point()));
        
        #endregion

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
    }
}

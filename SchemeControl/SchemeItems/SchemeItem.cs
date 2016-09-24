using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Item in scheme
    /// </summary>
    [TemplatePart(Name = "PART_DragAnchor", Type=typeof(FrameworkElement))]
    public class SchemeItem : ContentControl
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(SchemeItem), new PropertyMetadata(0.0d));

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(SchemeItem), new PropertyMetadata(0.0d));

        public static readonly DependencyProperty ParentSchemeControlProperty =
            DependencyProperty.Register("ParentSchemeControl", typeof(SchemeControl), typeof(SchemeItem), new PropertyMetadata(null));

        #endregion

        /// <summary>
        /// X position of scheme item
        /// </summary>
        [Category("Layout")]
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        /// <summary>
        /// Y position of scheme item
        /// </summary>
        [Category("Layout")]
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        /// <summary>
        /// Scheme control which contains this scheme item. (Set in default style)
        /// </summary>
        public SchemeControl ParentSchemeControl
        {
            get { return (SchemeControl)GetValue(ParentSchemeControlProperty); }
            set { SetValue(ParentSchemeControlProperty, value); }
        }

        static SchemeItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SchemeItem), new FrameworkPropertyMetadata(typeof(SchemeItem)));
        }

        public SchemeItem()
        {
            this.Loaded += SchemeItem_Loaded;
        }

        #region Private Members
        // It is FrameworkElement which is anchor for dragging SchemeItem element.
        private FrameworkElement _dragAnchor;

        // If it is dragging now.
        private bool _dragging;

        private Point _lastMousePos;
        private Point _curMousePos;

        /// <summary>
        /// Mouse left button clicke down to drag anchor. We must start dragging.
        /// </summary>
        void _dragAnchor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragging = true;
            _lastMousePos = e.GetPosition(this.ParentSchemeControl);
            Mouse.Capture(_dragAnchor, CaptureMode.Element);
            this.SetValue(Canvas.ZIndexProperty, 20);
            if (this.ParentSchemeControl.SelectedItem == null || !this.ParentSchemeControl.SelectedItem.Equals(this.DataContext))
                this.ParentSchemeControl.SelectedItem = this.DataContext;
        }

        /// <summary>
        /// Mouse left button clicke up to drag anchor. We must stop dragging.
        /// </summary>
        void _dragAnchor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
            this.SetValue(Canvas.ZIndexProperty, 0);
            Mouse.Capture(_dragAnchor, CaptureMode.None);
        }

        /// <summary>
        /// Stop dragging
        /// </summary>
        void _dragAnchor_MouseLeave(object sender, MouseEventArgs e)
        {
            _dragging = false;
            this.SetValue(Canvas.ZIndexProperty, 0);
            Mouse.Capture(_dragAnchor, CaptureMode.None);
        }

        /// <summary>
        /// Mouse moved over drag anchor. We must do drag(if necessary)
        /// </summary>
        void _dragAnchor_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                _curMousePos = e.GetPosition(this.ParentSchemeControl);
                this.X += _curMousePos.X - _lastMousePos.X;
                this.Y += _curMousePos.Y - _lastMousePos.Y;
                _lastMousePos = _curMousePos;
            }
        }

        /// <summary>
        /// Raised when scheme item loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SchemeItem_Loaded(object sender, RoutedEventArgs e)
        {
            // If _dragAnchor is set (for example we change template) then unsubscribe for events
            if (_dragAnchor != null)
                _dragAnchor.MouseLeftButtonDown -= _dragAnchor_MouseLeftButtonDown;
            this._dragAnchor = GetTemplateChild("PART_DragAnchor") as FrameworkElement;
            if (_dragAnchor != null)
            {
                _dragAnchor.MouseLeftButtonDown += _dragAnchor_MouseLeftButtonDown;
                _dragAnchor.MouseLeftButtonUp += _dragAnchor_MouseLeftButtonUp;
                _dragAnchor.MouseMove += _dragAnchor_MouseMove;
                _dragAnchor.MouseLeave += _dragAnchor_MouseLeave;
            }
        }
        #endregion
    }
}

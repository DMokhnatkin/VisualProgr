using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace VisualProgrGUI.behavior
{
    public class ScrollViewerRightButtonScrollBehavior : Behavior<ScrollViewer>
    {
        public Cursor ScrollingCursor { get; set; }

        // Is scrolling now
        bool _isScrolling;
        bool isScrolling
        {
            get { return _isScrolling; }
            set
            {
                // Change cursor and capture mouse when we start scrolling
                if (value && !isScrolling)
                {
                    if (ScrollingCursor != null)
                    {
                        _normalCursor = this.AssociatedObject.Cursor;
                        this.AssociatedObject.Cursor = ScrollingCursor;

                    }
                    this.AssociatedObject.CaptureMouse();
                    _isScrolling = true;
                }
                // Change cursor to normal when scrolling ends
                if (!value && isScrolling)
                {
                    if (ScrollingCursor != null)
                    {
                        this.AssociatedObject.Cursor = _normalCursor;
                    }
                    this.AssociatedObject.ReleaseMouseCapture();
                    _isScrolling = false;
                }
            }
        }

        Cursor _normalCursor; // We will store current cursor before change it

        Point _prevMousePos; // Previous frame mouse position

        Point _curMousePos; // Curent frame mouse position

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseRightButtonDown += AssociatedObject_MouseRightButtonDown;
            this.AssociatedObject.MouseRightButtonUp += AssociatedObject_MouseRightButtonUp;
            this.AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }

        /// <summary>
        /// If mouse leave object, stop scrolling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            isScrolling = false;
        }

        /// <summary>
        /// Stop scrolling wgen right button up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isScrolling = false;
        }

        /// <summary>
        /// Start sccrolling when right button down
        /// </summary>
        void AssociatedObject_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isScrolling = true;
            _prevMousePos = Mouse.GetPosition(this.AssociatedObject);
        }

        /// <summary>
        /// If scrolling now then make scroll else ignore
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isScrolling)
            {
                _curMousePos = Mouse.GetPosition(this.AssociatedObject);
                this.AssociatedObject.ScrollToHorizontalOffset(this.AssociatedObject.HorizontalOffset + (_prevMousePos.X - _curMousePos.X));
                this.AssociatedObject.ScrollToVerticalOffset(this.AssociatedObject.VerticalOffset + (_prevMousePos.Y - _curMousePos.Y));
                _prevMousePos = _curMousePos;
            }    
        }
    }
}

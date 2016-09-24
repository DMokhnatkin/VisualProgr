using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Controls;

namespace VisualProgr.GUI.elements
{
    /// <summary>
    /// Реализация поведения перетаскивания внутри canvas
    /// </summary>
    public class DragInCanvasBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// Элемент который захватывается мышкой
        /// </summary>
        private FrameworkElement _thumb;

        /// <summary>
        /// Перетаскиваемый элемент
        /// </summary>
        private FrameworkElement _draggable;

        /// <summary>
        /// Выполянется ли смещение
        /// </summary>
        private bool _moving;

        /// <summary>
        /// Последняя позиция курсора
        /// </summary>
        Point _lastMousePos;

        /// <param name="thumb">Элемент который захватывается мышкой</param>
        /// <param name="draggable">Перетаскиваемый элемент</param>
        public DragInCanvasBehavior(FrameworkElement thumb, FrameworkElement draggable)
        {
            _thumb = thumb;
            _draggable = draggable;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            _thumb.MouseLeftButtonDown += _thumb_MouseLeftButtonDown;
            _thumb.MouseLeftButtonUp += _thumb_MouseLeftButtonUp;
            _thumb.MouseMove += _thumb_MouseMove;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            _thumb.MouseLeftButtonDown -= _thumb_MouseLeftButtonDown;
            _thumb.MouseLeftButtonUp -= _thumb_MouseLeftButtonUp;
            _thumb.MouseMove -= _thumb_MouseMove;
        }

        /// <summary>
        /// При щелчке левой кнопкой мыши на thumb начинаем смещение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _thumb_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _moving = true;
            _lastMousePos = e.GetPosition(_thumb);
            _thumb.CaptureMouse();
            if (double.IsNaN((double)_draggable.GetValue(Canvas.LeftProperty)))
                _draggable.SetValue(Canvas.LeftProperty, 0.0d);
            if (double.IsNaN((double)_draggable.GetValue(Canvas.TopProperty)))
                _draggable.SetValue(Canvas.TopProperty, 0.0d);
            e.Handled = true;
        }

        /// <summary>
        /// Если мышка с thumb была отпущена, то заканчиваем смещение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _thumb_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _moving = false;
            _thumb.ReleaseMouseCapture();
            e.Handled = true;
        }

        /// <summary>
        /// При смещении курсора, если кнопка не отпущена, то перемещаем контроллер 
        /// на столько же на сколько переместился курсор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _thumb_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_moving)
            {
                double newX = (double)_draggable.GetValue(Canvas.LeftProperty) +
                              e.GetPosition(_thumb).X -
                              _lastMousePos.X;
                double newY = (double)_draggable.GetValue(Canvas.TopProperty) +
                              e.GetPosition(_thumb).Y -
                              _lastMousePos.Y;
                if (newX < 0)
                    newX = 0;
                if (newY < 0)
                    newY = 0;
                _draggable.SetValue(Canvas.LeftProperty, newX);
                _draggable.SetValue(Canvas.TopProperty, newY);
                _lastMousePos = e.GetPosition(_thumb);
            }
            e.Handled = true;
        }

    }
}

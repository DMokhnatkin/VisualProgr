using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualProgr;
using VisualProgr.GUI.elements.ports;

namespace VisualProgr.GUI.elements
{
    /// <summary>
    /// Логика взаимодействия для WPF_Node.xaml
    /// </summary>
    public partial class WPF_Node : UserControl
    {

        #region DependencyProperty Ссылка на ноду (Node)
        public static readonly DependencyProperty _node = DependencyProperty.Register("Node",
            typeof(Node), typeof(WPF_Node), new PropertyMetadata(new PropertyChangedCallback(_nodeChanged)));

        private static void _nodeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as WPF_Node).SP_Inputs.Children.Clear();
            foreach (Port port in (e.NewValue as Node).Ports.Inputs)
            {
                VIS_Port t = new VIS_Port();
                t.Port = port;
                t.Style = (sender as WPF_Node).Resources["InputPort"] as Style;
                t.VIS_Node = sender as WPF_Node;
                t.Name = port.Name;
                (sender as WPF_Node).SP_Inputs.Children.Add(t);
                t.ApplyTemplate();
            }
            (sender as WPF_Node).SP_Outputs.Children.Clear();
            foreach (Port port in (e.NewValue as Node).Ports.Outputs)
            {
                VIS_Port t = new VIS_Port();
                t.Port = port;
                t.Style = (sender as WPF_Node).Resources["OutputPort"] as Style;
                t.VIS_Node = sender as WPF_Node;
                t.Name = port.Name;
                (sender as WPF_Node).SP_Outputs.Children.Add(t);
                t.ApplyTemplate();
            }
        }

        public Node Node
        {
            get { return (Node)GetValue(_node); }
            set { SetValue(_node, value); }
        }
        #endregion

        #region DependencyProperty Выбрана ли нода (NodeSelected)
        public static readonly DependencyProperty NodeSelectedProperty = DependencyProperty.Register("NodeSelected",
            typeof(bool), typeof(WPF_Node), new PropertyMetadata(new PropertyChangedCallback(NodeSelectedChangedHandler)));
        /// <summary>
        /// Вызываем событие NodeSelected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void NodeSelectedChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as UIElement).RaiseEvent(new RoutedEventArgs(NodeSelectedEvent, sender));
        }
        public bool NodeSelected
        {
            get { return (bool)GetValue(NodeSelectedProperty); }
            set { SetValue(NodeSelectedProperty, value); }
        }
        #endregion

        #region RoutedEvent Событие выбора этой ноды (NodeSelectedEvent)
        public static readonly RoutedEvent NodeSelectedEvent = EventManager.RegisterRoutedEvent("NodeSelected",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WPF_Node));

        public event RoutedEventHandler NodeSelectedChanged
        {
            add { AddHandler(NodeSelectedEvent, value); }
            remove { RemoveHandler(NodeSelectedEvent, value); }
        }
        #endregion

        #region DependencyProperty Кисть фона топ бара ноды (TopBarBrush)
        public static readonly DependencyProperty TopBarBrushProperty = DependencyProperty.Register("TopBarBrush",
            typeof(Brush), typeof(WPF_Node),
            new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// Кисть топ бара ноды
        /// </summary>
        public Brush TopBarBrush
        {
            get { return (Brush)GetValue(TopBarBrushProperty); }
            set { SetValue(TopBarBrushProperty, (Brush)value); }
        }
        #endregion

        #region Перемещение элемента мышкой
        public delegate void MovedHandler(object sender, double newLeft, double newRight);
        public event MovedHandler Moved;

        bool _movingWPF_Node = false;
        Point _lastMousePos;
        /// <summary>
        /// При щелчке левой кнопкой мыши на заголовок контроллера начинаем смещение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _movingWPF_Node = true;
            _lastMousePos = e.GetPosition(this.Parent as UIElement);
            (sender as FrameworkElement).CaptureMouse();
            if (double.IsNaN((double)this.GetValue(Canvas.LeftProperty)))
                this.SetValue(Canvas.LeftProperty, 0.0d);
            if (double.IsNaN((double)this.GetValue(Canvas.TopProperty)))
                this.SetValue(Canvas.TopProperty, 0.0d);
            NodeSelected = true;
            e.Handled = true;
        }

        /// <summary>
        /// Если мышка с котроллера была отпущена, то заканчиваем смещение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _movingWPF_Node = false;
            if (Mouse.Captured != null)
                Mouse.Captured.ReleaseMouseCapture();
            e.Handled = true;
        }

        /// <summary>
        /// При смещении курсора, если кнопка не отпущена, то перемещаем контроллер 
        /// на столько же на сколько переместился курсор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _movingWPF_Node)
            {
                double newX = (double)this.GetValue(Canvas.LeftProperty) +
                              e.GetPosition(this.Parent as FrameworkElement).X -
                              _lastMousePos.X;
                double newY = (double)this.GetValue(Canvas.TopProperty) +
                              e.GetPosition(this.Parent as FrameworkElement).Y -
                              _lastMousePos.Y;
                if (newX < 0)
                    newX = 0;
                if (newY < 0)
                    newY = 0;
                this.SetValue(Canvas.LeftProperty, newX);
                this.SetValue(Canvas.TopProperty, newY);
                _lastMousePos = e.GetPosition(this.Parent as FrameworkElement);
                if (Moved != null)
                    Moved(this, newX, newY);
            }
            e.Handled = true;
        }
        #endregion

        #region (GetVisPort) Получить порт
        public VIS_Port GetVisPort(string name)
        {
            foreach (var t in SP_Inputs.Children)
            {
                if ((t as VIS_Port).Name == name)
                    return t as VIS_Port;
            }
            foreach (var t in SP_Outputs.Children)
            {
                if ((t as VIS_Port).Name == name)
                    return t as VIS_Port;
            }
            return null;
        }
        #endregion

        public WPF_Node()
        {
            InitializeComponent();
        }

        private void BO_NodeTopBar_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //WPF_SchemeView.DeleteNodeCommand.Execute(this, this);
        }

    }
}

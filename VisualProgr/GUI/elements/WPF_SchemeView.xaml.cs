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
using System.Resources;
using System.IO;
using VisualProgr;
using System.Globalization;
using VisualProgr.GUI.elements.ports;

namespace VisualProgr.GUI.elements
{
    /// <summary>
    /// Логика взаимодействия для WPF_WPF_SchemeView.xaml
    /// </summary>
    public partial class WPF_SchemeView : UserControl
    {
        #region DependencyProperty Выбранная нода (SelectedNode)
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode",
            typeof(WPF_Node), typeof(WPF_SchemeView), new PropertyMetadata(new PropertyChangedCallback(SelectedNodeChanged)));

        static void SelectedNodeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                (e.OldValue as WPF_Node).NodeSelected = false;
                (e.OldValue as WPF_Node).Node.AutoUpdate = false;
            }
            if (e.NewValue != null)
            {
                (e.NewValue as WPF_Node).NodeSelected = true;
                (e.NewValue as WPF_Node).Node.AutoUpdate = true;
                (e.NewValue as WPF_Node).Node.Update();
            }
        }

        public WPF_Node SelectedNode
        {
            get { return (WPF_Node)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }
        #endregion

        #region (CreateConnection) Создать соединение
        private class MiddlePointConverterX : IValueConverter
        {
            public object Convert(object value, Type targetType,
                                  object parameter, CultureInfo culture)
            {
                var canvas = (Canvas)parameter;
                return (value as FrameworkElement).TranslatePoint(new Point((value as FrameworkElement).ActualWidth / 2.0d, (value as FrameworkElement).ActualHeight / 2.0d), canvas).X;
            }

            public object ConvertBack(object value, Type targetType,
                                      object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        private class MiddlePointConverterY : IValueConverter
        {
            public object Convert(object value, Type targetType,
                                  object parameter, CultureInfo culture)
            {
                var canvas = (Canvas)parameter;
                return (value as FrameworkElement).TranslatePoint(new Point((value as FrameworkElement).ActualWidth / 2.0d, (value as FrameworkElement).ActualHeight / 2.0d), canvas).Y;
            }

            public object ConvertBack(object value, Type targetType,
                                      object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        /// <summary>
        /// Команда на создание соединения
        /// </summary>
        public static readonly RoutedCommand CreateConnectionCommand = new RoutedCommand();

        //Ссылка на элемент управления
        VIS_Port _firstElement = null;
        Line _line = new Line();

        //Начать создавать соединение
        void StartCreation(VIS_Port element)
        {
            _line = new Line();
            _firstElement = element;
            _firstElement.Active = true;
            BindingOperations.SetBinding(_line, Line.X1Property, new Binding
            {
                Source = this._firstElement.GetConnectElement(),
                Converter = new MiddlePointConverterX(),
                ConverterParameter = canvas,
                Mode = BindingMode.OneWay
            });
            BindingOperations.SetBinding(_line, Line.Y1Property, new Binding
            {
                Source = this._firstElement.GetConnectElement(),
                Converter = new MiddlePointConverterY(),
                ConverterParameter = canvas,
                Mode = BindingMode.OneWay
            });
            _line.X2 = Mouse.GetPosition(canvas).X;
            _line.Y2 = Mouse.GetPosition(canvas).Y;
            _line.Stroke = new SolidColorBrush(Colors.Black);
            _line.IsHitTestVisible = false;
            _line.SetValue(Canvas.ZIndexProperty, -10);
            this.canvas.Children.Add(_line);
        }

        void EndCreation(VIS_Port el2)
        {
            Scheme.Cur.Connect(_firstElement.Port, el2.Port);
            BindingOperations.SetBinding(_line, Line.X2Property, new Binding
            {
                Source = el2.GetConnectElement(),
                Converter = new MiddlePointConverterX(),
                ConverterParameter = canvas,
                Mode = BindingMode.OneWay
            });
            BindingOperations.SetBinding(_line, Line.Y2Property, new Binding
            {
                Source = el2.GetConnectElement(),
                Converter = new MiddlePointConverterY(),
                ConverterParameter = canvas,
                Mode = BindingMode.OneWay
            });
            _firstElement.Active = false;
            el2.Active = false;
            _firstElement = null;
            UpdateLines();
        }

        /// <summary>
        /// Сбросить создание связи
        /// </summary>
        private void BreakConnectCreation()
        {
            if (_firstElement != null)
            {
                _firstElement.Active = false;
                _firstElement = null;
            }
            this.canvas.Children.Remove(_line);
        }

        /// <summary>
        /// Обработчик команды
        /// </summary>
        private void CreateConnectionExcecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //Создание соединения ещё не началось
            if (_firstElement == null)
            {
                StartCreation((VIS_Port)e.Parameter);
                return;
            }
            //Ожидается ввод 2-го порта, но нажимается на тот же самый
            if (_firstElement == (VIS_Port)e.Parameter)
            {
                _firstElement.Active = false;
                _firstElement = null;
                this.canvas.Children.Remove(_line);
                return;
            }
            //Ожидается ввод 2-го порта, но был нажат порт того же типа
            if ((_firstElement.Port is Input && ((VIS_Port)e.Parameter).Port is Input) || 
                (_firstElement.Port is Output && ((VIS_Port)e.Parameter).Port is Output))
            {
                BreakConnectCreation();
            }
            if (_firstElement != (VIS_Port)e.Parameter)
            {
                try
                {
                    EndCreation((VIS_Port)e.Parameter);
                }
                catch 
                {
                    BreakConnectCreation();
                }
                return;
            }
        }

        /// <summary>
        /// При перемещении мыши надо обновить Line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewArea_ConnCreateMouseMove(object sender, MouseEventArgs e)
        {
            if (_firstElement != null)
            {
                _line.X2 = Mouse.GetPosition(canvas).X;
                _line.Y2 = Mouse.GetPosition(canvas).Y;
            }
        }
        
        public void UpdateLines()
        {
            foreach (UIElement t in canvas.Children)
            {
                if (t is Line)
                {
                    BindingExpression bind;
                    if ((bind = (t as Line).GetBindingExpression(Line.X1Property)) != null)
                        bind.UpdateTarget();
                    if ((bind = (t as Line).GetBindingExpression(Line.Y1Property)) != null)
                        bind.UpdateTarget();
                    if ((bind = (t as Line).GetBindingExpression(Line.X2Property)) != null)
                        bind.UpdateTarget();
                    if ((bind = (t as Line).GetBindingExpression(Line.Y2Property)) != null)
                        bind.UpdateTarget();
                }
            }
        }

        /// <summary>
        /// При перемещении какой либо ноды обновляем линии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newLeft"></param>
        /// <param name="newRight"></param>
        private void NodeMoved(object sender, double newLeft, double newRight)
        {
            UpdateLines();
        }

        /// <summary>
        /// Соединить два порта
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void Connect(VIS_Port first, VIS_Port second)
        {
            Line _line = new Line();
            _line.SetValue(Canvas.ZIndexProperty, -10);
            _line.Stroke = new SolidColorBrush(Colors.Black);
            _line.IsHitTestVisible = false;
            Scheme.Cur.Connect(first.Port, second.Port);
            BindingOperations.SetBinding(_line, Line.X1Property, new Binding
            {
                Source = first.GetConnectElement(),
                Converter = new MiddlePointConverterX(),
                ConverterParameter = canvas,
                Mode = BindingMode.OneWay
            });
            BindingOperations.SetBinding(_line, Line.Y1Property, new Binding
            {
                Source = first.GetConnectElement(),
                Converter = new MiddlePointConverterY(),
                ConverterParameter = canvas,
                Mode = BindingMode.OneWay
            });
            BindingOperations.SetBinding(_line, Line.X2Property, new Binding
            {
                Source = second.GetConnectElement(),
                Converter = new MiddlePointConverterX(),
                ConverterParameter = canvas,
                Mode = BindingMode.OneWay
            });
            BindingOperations.SetBinding(_line, Line.Y2Property, new Binding
            {
                Source = second.GetConnectElement(),
                Converter = new MiddlePointConverterY(),
                ConverterParameter = canvas,
                Mode = BindingMode.OneWay
            });
            canvas.Children.Add(_line);
            _line.ApplyTemplate();
            UpdateLines();
        }
        #endregion

        #region (DeleteConnections) Удалить соединения
        /// <summary>
        /// Команда на удаление соединения
        /// </summary>
        public static readonly RoutedCommand DeleteConnectionCommand = new RoutedCommand();

        private void ExecutedDeleteConnection(object sender, ExecutedRoutedEventArgs e)
        {
            ConnectionManager.ClearConnections((e.Parameter as VIS_Port).Port);
            foreach (Line t in canvas.Children.OfType<Line>())
            {
                if (t.GetBindingExpression(Line.X1Property).DataItem == (e.Parameter as VIS_Port).GetConnectElement() ||
                    t.GetBindingExpression(Line.X2Property).DataItem == (e.Parameter as VIS_Port).GetConnectElement())
                {
                    canvas.Children.Remove(t);
                    break;
                }
            }
        }
        #endregion

        #region (DeleteNode) Удалить ноду
        /// <summary>
        /// Команда на удаление ноды
        /// </summary>
        public static readonly RoutedCommand DeleteNodeCommand = new RoutedCommand();

        private void ExecutedDeleteNode(object sender, ExecutedRoutedEventArgs e)
        {
            Scheme.Cur.DeleteNode((e.Parameter as WPF_Node).Node);
            canvas.Children.Remove(e.Parameter as WPF_Node);
            UpdateLines();
        }
        #endregion

        #region (Clear) Очистить
        public void Clear()
        {
            Scheme.Cur.Clear();
            canvas.Children.Clear();
        }
        #endregion

        #region (CreateNode) Создать ноду
        /// <summary>
        /// Создать ноду и её визуальный элемент
        /// </summary>
        /// <param name="type">Тип ноды</param>
        /// <param name="canvas">Родительский элемент ноды</param>
        /// <param name="topPos">Canvas.TopProperty</param>
        /// <param name="leftPos">Canvas.LeftProperty</param>
        /// <param name="name">Имя ноды</param>
        public WPF_Node CreateNode(NodeTypeMetaData type, double leftPos, double topPos, string name = null)
        {
            if (name == null)
                name = Scheme.Cur.GetFreeName();
            Node newNode = (Node)Activator.CreateInstance(type.SystemType, name);
            Scheme.Cur.AddNode(newNode);
            WPF_Node elNode = new WPF_Node();
            elNode.Node = newNode;
            elNode.SetValue(Canvas.LeftProperty, leftPos);
            elNode.SetValue(Canvas.TopProperty, topPos);
            elNode.Moved += NodeMoved;
            canvas.Children.Add(elNode);
            return elNode;
        }
        #endregion

        #region Скроллинг мышкой при нажатой левой кнопке
        /// <summary>
        /// Происходит ли скроллинг в данный момент
        /// </summary>
        bool _scrollScheme = false;
        /// <summary>
        /// Предыдущее положение курсора
        /// </summary>
        Point _lastMousePos;
        /// <summary>
        /// Скорость скроллинга
        /// </summary>
        const double _scrollSpeed = 0.8d;
        /// <summary>
        /// Начало скролинга
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastMousePos = e.GetPosition(viewArea);
            //Загружаем курсор руки
            ResourceManager resManager = new ResourceManager("VisualProgr.Resources.cursors.MyCursors", this.GetType().Assembly);
            MemoryStream stream = new MemoryStream(resManager.GetObject("closedhand") as byte[]);
            Cursor customCursor = new Cursor(stream);
            //Меняем курсор
            viewArea.Cursor = customCursor;
            viewArea.CaptureMouse();
            _scrollScheme = true;
        }

        /// <summary>
        /// Конец скроллинга
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            viewArea.Cursor = Cursors.Arrow;
            _scrollScheme = false;
            viewArea.ReleaseMouseCapture();
        }

        /// <summary>
        /// При перемещении мыши надо прокрутить ScrollViewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewArea_ScrollMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _scrollScheme)
            {
                viewArea.ScrollToHorizontalOffset(viewArea.HorizontalOffset - (e.GetPosition(viewArea).X - _lastMousePos.X) * _scrollSpeed);
                viewArea.ScrollToVerticalOffset(viewArea.VerticalOffset - (e.GetPosition(viewArea).Y - _lastMousePos.Y) * _scrollSpeed);
                _lastMousePos = e.GetPosition(viewArea);
            }
        }

        private void viewArea_LostMouseCapture(object sender, MouseEventArgs e)
        {
            canvas.Cursor = Cursors.Arrow;
        }
        #endregion

        #region Drag and drop
        private void canvas_Drop(object sender, DragEventArgs e)
        {
            NodeTypeMetaData type = (NodeTypeMetaData)e.Data.GetData("NodeTypeMetaData");
            CreateNode(type, e.GetPosition(canvas).X, e.GetPosition(canvas).Y);
        }
        #endregion

        #region RoutedEvent Событие выбора ноды (NodeSelectedEvent)
        public static readonly RoutedEvent NodeSelectedEvent = WPF_Node.NodeSelectedEvent.AddOwner(typeof(WPF_SchemeView));

        public event RoutedEventHandler NodeSelected
        {
            add { AddHandler(NodeSelectedEvent, value);}
            remove{RemoveHandler(NodeSelectedEvent, value);}
        }

        void WPF_SchemeView_NodeSelected(object sender, RoutedEventArgs e)
        {
            if (SelectedNode != null)
                SelectedNode.NodeSelected = false;
            SelectedNode = (e.OriginalSource as WPF_Node);
        }
        #endregion

        #region (FindVisElByNode) Поиск визуального элемента по ноде
        public WPF_Node GetVisEl(Node node)
        {
            foreach (WPF_Node k in canvas.Children.OfType<WPF_Node>())
                if (k.Node == node)
                    return k;
            return null;
        }

        public WPF_Node GetVisEl(string nodeName)
        {
            foreach (WPF_Node k in canvas.Children.OfType<WPF_Node>())
                if (k.Node.Name == nodeName)
                    return k;
            return null;
        }
        #endregion

        /// <summary>
        /// Вызовем все обработчики которые надо вызвать при перемещении мышки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (_scrollScheme)
                viewArea_ScrollMouseMove(sender, e);
            if (_firstElement != null)
                viewArea_ConnCreateMouseMove(sender, e);
        }

        public WPF_SchemeView()
        {
            InitializeComponent();
            this.NodeSelected += WPF_SchemeView_NodeSelected;
        }

    }
}

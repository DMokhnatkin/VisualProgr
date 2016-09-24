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

namespace VisualProgr.GUI.elements.ports
{
    public class VIS_Port : Control
    {

        #region Сслыка на порт (Port)
        public static readonly DependencyProperty PortProperty = DependencyProperty.Register("Port", 
            typeof(Port), typeof(VIS_Port));

        public Port Port
        {
            get { return (Port)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }
        #endregion

        #region Ссылка на визуальный элемент ноды (VIS_Node)
        public static readonly DependencyProperty VIS_NodeProperty = DependencyProperty.Register("VIS_Node",
            typeof(WPF_Node), typeof(VIS_Port));

        public WPF_Node VIS_Node
        {
            get { return (WPF_Node)GetValue(VIS_NodeProperty); }
            set { SetValue(VIS_NodeProperty, value); }
        }
        #endregion

        #region RoutedEvent Событие изменения активности (ActiveChanged)
        public static readonly RoutedEvent ActiveChangedEvent = EventManager.RegisterRoutedEvent("ActiveChanged",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VIS_Port));
        
        public event RoutedEventHandler ActiveChanged
        {
            add { AddHandler(ActiveChangedEvent, value); }
            remove { RemoveHandler(ActiveChangedEvent, value); }
        }

        public void OnActiveChanged()
        {
            RaiseEvent(new RoutedEventArgs(ActiveChangedEvent, this));
        }
        #endregion

        #region DependencyProperty Активный ли порт (Active)
        public static readonly DependencyProperty ActiveProperty = DependencyProperty.Register("Active",
            typeof(bool), typeof(VIS_Port), new PropertyMetadata(new PropertyChangedCallback(ActivePropertyChanged)));

        static void ActivePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as VIS_Port).OnActiveChanged();
        }

        public bool Active
        {
            get { return (bool)GetValue(ActiveProperty); }
            set { SetValue(ActiveProperty, value); }
        }
        #endregion

        #region Щелчек правой кнопкой мыши
        private void VIS_Port_RightButtonClick(object sender, MouseButtonEventArgs e)
        {
            WPF_SchemeView.DeleteConnectionCommand.Execute(this, this);
        }
        #endregion

        #region Щелчек левой кнопкой мыши
        private void VIS_Port_LeftButtonClick(object sender, MouseButtonEventArgs e)
        {
            WPF_SchemeView.CreateConnectionCommand.Execute(this, this);
        }
        #endregion

        /// <summary>
        /// Элемент к которому создается свзяь
        /// </summary>
        /// <returns></returns>
        public FrameworkElement GetConnectElement()
        {
            //return GetTemplateChild("ConnectionControl") as FrameworkElement;
            return (FrameworkElement)this.Template.FindName("ConnectionControl", this);
        }

        static VIS_Port()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VIS_Port), new FrameworkPropertyMetadata(typeof(VIS_Port)));
        }

        public VIS_Port()
        {
            this.AddHandler(FrameworkElement.MouseRightButtonDownEvent, new MouseButtonEventHandler(VIS_Port_RightButtonClick));
            this.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(VIS_Port_LeftButtonClick));
        }

    }
}

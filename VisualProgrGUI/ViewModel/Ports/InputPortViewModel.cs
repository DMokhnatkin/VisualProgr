using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrGUI.ViewModel
{
    public class InputPortViewModel : PortViewModel
    {
        protected static readonly DependencyProperty ValProperty =
            DependencyProperty.Register("Val", typeof(object), typeof(InputPortViewModel), new PropertyMetadata(new PropertyChangedCallback(ValChanged)));

        /// <summary>
        /// Value which can be changed using binding
        /// </summary>
        public object Val
        {
            get { return GetValue(ValProperty); }
            set { SetValue(ValProperty, value); }
        }

        /// <summary>
        /// Raised when val in this port changed by user
        /// </summary>
        static void ValChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = sender as InputPortViewModel;
            if (viewModel.Port.DataType == typeof(bool))
                viewModel.Port.SetValue(Convert.ToBoolean(e.NewValue));
            if (viewModel.Port.DataType == typeof(Int32))
                viewModel.Port.SetValue(Convert.ToInt32(e.NewValue));
        }

        public InputPortViewModel(Main.Ports.Port port1, NodeViewModel parent) : base(port1, parent)
        {
            Val = Port.GetValue();
        }
    }
}

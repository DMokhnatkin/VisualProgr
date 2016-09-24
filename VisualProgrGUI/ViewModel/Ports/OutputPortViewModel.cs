using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrGUI.ViewModel
{
    public class OutputPortViewModel : PortViewModel
    {
        protected static readonly DependencyPropertyKey ValPropertyKey =
            DependencyProperty.RegisterReadOnly("Val", typeof(object), typeof(OutputPortViewModel), new PropertyMetadata(null));
        public static readonly DependencyProperty ValProperty = ValPropertyKey.DependencyProperty;

        /// <summary>
        /// Value which can't be changed using binding
        /// </summary>
        public object Val
        {
            get { return GetValue(ValProperty); }
            protected set { SetValue(ValPropertyKey, value); }
        }

        public OutputPortViewModel(Main.Ports.Port port, NodeViewModel parent) : base(port, parent)
        {
            Val = Port.GetValue();
        }

        /// <summary>
        /// Calculate value in this port
        /// </summary>
        public void UpdateVal()
        {
            Val = Port.GetValue();
        }
    }
}

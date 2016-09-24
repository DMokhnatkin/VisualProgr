using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VisualProgrGUI.View
{
    public class ValTemplateSelector : DataTemplateSelector
    {
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var _item = item as FrameworkElement;
            Contract.Requires<ArgumentException>(item != null);
            return null;
        }
    }
}

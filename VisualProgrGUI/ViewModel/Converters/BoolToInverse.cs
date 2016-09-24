using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VisualProgrGUI.ViewModel.Converters
{
    /// <summary>
    /// Inverse bool
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolToInverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return !(bool)value;
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return !(bool)value;
            else
                return null;
        }
    }
}

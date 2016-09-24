using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SchemeControlLib
{
    static class FindAncestor
    {
        public static DependencyObject FindAncestorOfType(DependencyObject obj, Type type)
        {
            if (obj.GetType() == type)
                return obj;
            else
                return FindAncestorOfType(VisualTreeHelper.GetParent(obj), type);
        }
    }
}

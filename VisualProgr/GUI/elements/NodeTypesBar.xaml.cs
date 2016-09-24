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

namespace VisualProgr.GUI.elements
{
    /// <summary>
    /// Логика взаимодействия для NodeTypesBar.xaml
    /// </summary>
    public partial class NodeTypesBar : UserControl
    {
        public NodeTypesBar()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(sender as DependencyObject, (sender as FrameworkElement).Tag , DragDropEffects.Copy);
        }
    }
}

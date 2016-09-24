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

namespace VisualProgr.GUI.elements
{
    /// <summary>
    /// Логика взаимодействия для VIS_Values.xaml
    /// </summary>
    public partial class VIS_Values : UserControl
    {
        #region DependencyProperty Ссылка на ноду (Node)
        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register("Node",
            typeof(Node), typeof(VIS_Values));

        public Node Node
        {
            get { return (Node)GetValue(NodeProperty); }
            set { SetValue(NodeProperty, value); }
        }
        #endregion

        public VIS_Values()
        {
            InitializeComponent();
        }
    }
}

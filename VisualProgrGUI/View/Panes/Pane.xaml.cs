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

namespace VisualProgrGUI.View.Panes
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class Pane : UserControl
    {
        #region Dependency Properties
        public static readonly DependencyProperty ContainsProperty =
            DependencyProperty.Register("Contains", typeof(object), typeof(Pane), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(Pane), new PropertyMetadata(null));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(Pane), new PropertyMetadata(""));
        #endregion

        /// <summary>
        /// Background of header
        /// </summary>
        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        /// <summary>
        /// Label of this pane
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Content of this pane
        /// </summary>
        public object Contains
        {
            get { return (object)GetValue(ContainsProperty); }
            set { SetValue(ContainsProperty, value); }
        }

        public Pane()
        {
            InitializeComponent();
        }
    }
}

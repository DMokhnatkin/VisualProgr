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

namespace VisualProgr.GUI.elements.valControls
{
    /// <summary>
    /// Логика взаимодействия для VIS_Boolean.xaml
    /// </summary>
    public partial class VIS_Boolean : UserControl
    {
        #region DependencyProperty Можно ли редактировать (CanBeEdited)
        public readonly static DependencyProperty CanBeEditedProperty = DependencyProperty.Register("CanBeEdited",
            typeof(bool), typeof(VIS_Boolean));
        public bool CanBeEdited
        {
            get { return (bool)GetValue(CanBeEditedProperty); }
            set { SetValue(CanBeEditedProperty, value); }
        }
        #endregion

        #region DependencyProperty Значение к которому привязан элемент управления (BindingVal)
        public static readonly DependencyProperty BindingValProperty = DependencyProperty.Register("BindingVal",
            typeof(bool), typeof(VIS_Boolean));
        public bool  BindingVal
        {
            get { return (bool)GetValue(BindingValProperty); }
            set { SetValue(BindingValProperty, value); }
        }
        #endregion

        public VIS_Boolean()
        {
            InitializeComponent();
        }
    }
}

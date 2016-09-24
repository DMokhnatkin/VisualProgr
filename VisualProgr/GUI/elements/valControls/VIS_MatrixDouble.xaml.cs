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
using MathNet.Numerics.LinearAlgebra;

namespace VisualProgr.GUI.elements.valControls
{
    /// <summary>
    /// Логика взаимодействия для VIS_Boolean.xaml
    /// </summary>
    public partial class VIS_MatrixDouble : UserControl
    {
        #region DependencyProperty Можно ли редактировать (CanBeEdited)
        public readonly static DependencyProperty CanBeEditedProperty = DependencyProperty.Register("CanBeEdited",
            typeof(bool), typeof(VIS_MatrixDouble));
        public bool CanBeEdited
        {
            get { return (bool)GetValue(CanBeEditedProperty); }
            set { SetValue(CanBeEditedProperty, value); }
        }
        #endregion

        #region DependencyProperty Значение к которому привязан элемент управления (BindingVal)
        public static readonly DependencyProperty BindingValProperty = DependencyProperty.Register("BindingVal",
            typeof(MathNet.Numerics.LinearAlgebra.Matrix<double>), typeof(VIS_MatrixDouble), new PropertyMetadata(new PropertyChangedCallback(BindingValChanged)));

        private static void BindingValChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var VIS_matr = (sender as VIS_MatrixDouble);
            var matrix = VIS_matr.BindingVal;
            for (int i = 0; i < matrix.RowCount; i++)
            {
                RowDefinition t = new RowDefinition();
                t.Height = GridLength.Auto;
                VIS_matr.matrixGrid.RowDefinitions.Add(t);
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    ColumnDefinition col = new ColumnDefinition();
                    col.Width = GridLength.Auto;
                    VIS_matr.matrixGrid.ColumnDefinitions.Add(col);
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "text";
                    VIS_matr.AddChild(textBlock);
                    textBlock.SetValue(Grid.RowProperty, i);
                    textBlock.SetValue(Grid.ColumnProperty, j);
                }
            }
        }

        public Matrix<double> BindingVal
        {
            get { return (Matrix<double>)GetValue(BindingValProperty); }
            set { SetValue(BindingValProperty, value); }
        }
        #endregion

        public VIS_MatrixDouble()
        {
            InitializeComponent();
        }
    }
}

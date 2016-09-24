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
using System.Windows.Shapes;
using VisualProgrGUI.ViewModel;

namespace VisualProgrGUI
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        void SetHighligh(TextBox text_box, bool flag)
        {
            if (flag)
            {
                text_box.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
                text_box.FontFamily = new FontFamily("Segoe UI Light");
            }
            else
            {
                text_box.Foreground = new SolidColorBrush(Colors.Black);
                text_box.FontFamily = new FontFamily("Segoe UI");
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "User name")
            {
                (sender as TextBox).Text = "";
                SetHighligh(sender as TextBox, false);
            }
            (DataContext as AuthorizationWindowViewModel).Message = "";
        }

        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "Password")
            {
                (sender as TextBox).Text = "";
                SetHighligh(sender as TextBox, false);
            }
            (DataContext as AuthorizationWindowViewModel).Message = "";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                SetHighligh(sender as TextBox, true);
                (sender as TextBox).Text = "User name";
            }
        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                SetHighligh(sender as TextBox, true);
                (sender as TextBox).Text = "Password";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as AuthorizationWindowViewModel).AuthorizateCommand.Execute(new Tuple<string, string, Window>(TB_UserName.Text, TB_Password.Text, this));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (DataContext as AuthorizationWindowViewModel).CreateUserCommand.Execute(new Tuple<string, string>(TB_UserName.Text, TB_Password.Text));
        }
    }
}

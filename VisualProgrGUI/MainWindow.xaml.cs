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
using System.Reflection;
using VisualProgrGUI.ViewModel;
using Microsoft.Win32;
using System.IO;
using System.Globalization;

namespace VisualProgrGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Main.Sceme.Scheme _scene;
        Main.Nodes.NodeTypesManager _nodeTypes;

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            switch (CultureInfo.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\languages\\StringResources.xaml", UriKind.Relative);
                    break;
                case "ru-RU":
                    dict.Source = new Uri("..\\Resources\\languages\\StringResources.rus.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\languages\\StringResources.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        public MainWindow()
        {
            _scene = new Main.Sceme.Scheme("scene1");
            InitializeComponent();
            SetLanguageDictionary();
        }

        private void SchemeArea_ConnectionCreated(object sender, SchemeControlLib.ConnectionCreatedEventArgs e)
        {
            e.CanBeCreated = ((sender as SchemeControlLib.SchemeControl).DataContext as WorkAreaViewModel).CreateConnection(e.First.DataContext as PortViewModel, e.Second.DataContext as PortViewModel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = "pr";
            dialog.Filter = "Scheme file (*.pr)|*.pr";
            if (dialog.ShowDialog() == true)
            {
                using (Stream str = dialog.OpenFile())
                {
                    var _viewModel = DataContext as MainWindowViewModel;
                    if (_viewModel.LoadSchemeCommand.CanExecute(str))
                        _viewModel.LoadSchemeCommand.Execute(str);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "pr";
            dialog.Filter = "Scheme file (*.pr)|*.pr";
            if (dialog.ShowDialog() == true)
            {
                using (Stream str = File.Create(dialog.FileName))
                {
                    var _viewModel = DataContext as MainWindowViewModel;
                    if (_viewModel.SaveSchemeCommand.CanExecute(str))
                        _viewModel.SaveSchemeCommand.Execute(str);
                }
            }
        }

    }
}

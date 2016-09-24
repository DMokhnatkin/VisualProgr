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
using System.Windows.Shapes;
using VisualProgr.GUI.elements;

namespace VisualProgr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            NodeTypes.Register(new NodeTypeMetaData("AND", typeof(NodeBool_AND),
                new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\nodes\\icons\\Bool_And.png")),
                "Boolean",
                new SolidColorBrush(Color.FromRgb(51,102,51))));
            NodeTypes.Register(new NodeTypeMetaData("OR", typeof(NodeBool_OR),
                new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\nodes\\icons\\Bool_Or.png")),
                "Boolean",
                new SolidColorBrush(Color.FromRgb(51,102,51))));
            NodeTypes.Register(new NodeTypeMetaData("CONST", typeof(NodeBool_Const),
                new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\nodes\\icons\\Bool_Const.png")),
                "Boolean",
                new SolidColorBrush(Color.FromRgb(51,102,51))));
            NodeTypes.Register(new NodeTypeMetaData("NOT", typeof(NodeBool_NOT),
                new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\nodes\\icons\\Bool_NOT.png")),
                "Boolean",
                new SolidColorBrush(Color.FromRgb(51,102,51))));
            NodeTypes.Register(new NodeTypeMetaData("XOR", typeof(NodeBool_XOR),
                new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\nodes\\icons\\Bool_XOR.png")),
                "Boolean",
                new SolidColorBrush(Color.FromRgb(51, 102, 51))));
            NodeTypes.Register(new NodeTypeMetaData("EQUALS", typeof(NodeBool_EQUALS),
                new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\nodes\\icons\\Bool_EQUALS.png")),
                "Boolean",
                new SolidColorBrush(Color.FromRgb(51, 102, 51))));
            NodeTypes.Register(new NodeTypeMetaData("notEQUALS", typeof(NodeBool_notEQUALS),
                new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\nodes\\icons\\Bool_notEQUALS.png")),
                "Boolean",
                new SolidColorBrush(Color.FromRgb(51, 102, 51))));
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.Data.CollectionViewSource nodeTypesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("nodeTypesViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // nodeTypesViewSource.Source = [универсальный источник данных]
        }

        #region SAveCommand
        public static RoutedCommand SaveCommand = new RoutedCommand();

        private void ExecutedSaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Xml file (*.xml)| *.xml";
            if (dlg.ShowDialog() == true)
            {
                VisualProgr.data.XML_SchemeSaver.SaveScheme(SchemeArea, dlg.FileName);
            }
        }
        #endregion

        #region LoadCommand
        public static RoutedCommand LoadCommand = new RoutedCommand();

        private void ExecutedLoadCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Xml file (*.xml)| *.xml";
            if (dlg.ShowDialog() == true)
            {
                VisualProgr.data.XML_SchemeLoader.Load(SchemeArea, dlg.FileName);
                SchemeArea.UpdateLines();
            }
        }
        #endregion
    }
}

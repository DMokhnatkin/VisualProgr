using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using System.Collections.ObjectModel;
using VisualProgrGUI.ViewModel.NodeTypes;
using VisualProgrGUI.ViewModel.Commands;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace VisualProgrGUI.ViewModel
{
    public class MainWindowViewModel : DependencyObject
    {
        #region View models
        /// <summary>
        /// Node types view model
        /// </summary>
        public NodeTypesListViewModel NodeTypes { get; set; }

        /// <summary>
        /// Work area view model
        /// </summary>
        public WorkAreaViewModel WorkArea { get; private set; }

        /// <summary>
        /// Console view model
        /// </summary>
        public ConsViewModel MyConsole { get; private set; }
        #endregion

        Main.Nodes.NodeTypesManager _nodeTypes;    

        public MainWindowViewModel()
        {
            List<Assembly> _assemblys = new List<Assembly>();
            _assemblys.Add(Assembly.LoadFrom(@"StandartNodes.dll"));
            _nodeTypes = new Main.Nodes.NodeTypesManager(_assemblys);

            WorkArea = new WorkAreaViewModel(_nodeTypes);

            NodeTypes = new NodeTypesListViewModel(_nodeTypes);

            MyConsole = new ConsViewModel();

            CalculateSchemeCommand = new SimpleCommand(CalculateSchemeExcecuted);
            ClearSchemeCommand = new SimpleCommand(ClearSchemeExcecuted);
            SaveSchemeCommand = new SimpleCommand(SaveSchemeExcecuted);
            LoadSchemeCommand = new SimpleCommand(LoadSchemeExcecuted);
        }

        #region Commands
        
        public SimpleCommand CalculateSchemeCommand { get; private set; }

        public SimpleCommand ClearSchemeCommand { get; private set; }

        public SimpleCommand SaveSchemeCommand { get; private set; }

        public SimpleCommand LoadSchemeCommand { get; private set; }
        #endregion
        
        /// <summary>
        /// Calculate current scene
        /// </summary>
        void CalculateSchemeExcecuted(object parameter)
        {
            this.WorkArea.CalculateScene();
        }

        /// <summary>
        /// Clear current scene
        /// </summary>
        void ClearSchemeExcecuted(object parameter)
        {
            this.WorkArea.ClearNodes();
        }

        /// <summary>
        /// Save current scheme
        /// </summary>
        void SaveSchemeExcecuted(object parameter)
        {
            if (parameter is Stream)
            {
                this.WorkArea.Save(parameter as Stream);
            }
        }

        /// <summary>
        /// Load scheme from stream
        /// </summary>
        void LoadSchemeExcecuted(object parameter)
        {
            if (parameter is Stream)
            {
                WorkArea.Load(parameter as Stream, WorkArea.Scheme.NodeTypes);
            }
        }
    }
}

using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using VisualProgrGUI.ViewModel.Commands;
using System.Linq;

namespace VisualProgrGUI.ViewModel
{
    public class NodeViewModel : DependencyObject
    {

        #region Dependency properties

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(NodeViewModel));

        public static readonly DependencyProperty NodeTypeNameProperty =
            DependencyProperty.Register("NodeTypeName", typeof(string), typeof(NodeViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty InputPortsProperty =
            DependencyProperty.Register("InputPorts", typeof(ObservableCollection<InputPortViewModel>),
                                        typeof(NodeViewModel), new PropertyMetadata(new ObservableCollection<InputPortViewModel>()));

        public static readonly DependencyProperty OutputPortsProperty =
            DependencyProperty.Register("OutputPorts", typeof(ObservableCollection<OutputPortViewModel>),
                                        typeof(NodeViewModel), new PropertyMetadata(new ObservableCollection<OutputPortViewModel>()));

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double), typeof(NodeViewModel), new PropertyMetadata(0.0d));

        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(double), typeof(NodeViewModel), new PropertyMetadata(0.0d));

        public static readonly DependencyProperty IsUpToDateProperty =
            DependencyProperty.Register("IsUpToDate", typeof(bool), typeof(NodeViewModel), new PropertyMetadata(false)); 
        #endregion

        /// <summary>
        /// Work area which contains this node
        /// </summary>
        public WorkAreaViewModel Parent { get; private set; }

        /// <summary>
        /// Name of node
        /// </summary>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Type of node
        /// </summary>
        public string NodeTypeName
        {
            get { return (string)GetValue(NodeTypeNameProperty); }
            set { SetValue(NodeTypeNameProperty, value); }
        }

        /// <summary>
        /// Input ports
        /// </summary>
        public ObservableCollection<InputPortViewModel> InputPorts
        {
            get { return (ObservableCollection<InputPortViewModel>)this.GetValue(InputPortsProperty); }
            private set { SetValue(InputPortsProperty, value); }
        }

        /// <summary>
        /// Output ports
        /// </summary>
        public ObservableCollection<OutputPortViewModel> OutputPorts
        {
            get { return (ObservableCollection<OutputPortViewModel>)GetValue(OutputPortsProperty); }
            set { SetValue(OutputPortsProperty, value); }
        }

        /// <summary>
        /// Node's left position
        /// </summary>
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        /// <summary>
        /// Node's right position
        /// </summary>
        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        /// <summary>
        /// Is this node is up to date
        /// </summary>
        public bool IsUpToDate
        {
            get { return (bool)GetValue(IsUpToDateProperty); }
            set { SetValue(IsUpToDateProperty, value); }
        }

        public Main.Nodes.Node NodeModel { get; private set; }

        public NodeViewModel(Main.Nodes.Node model, double left, double top, WorkAreaViewModel parent)
        {
            Parent = parent;
            NodeModel = model;
            Name = NodeModel.Name;
            NodeTypeName = model.GetType().GetField("typeName").GetValue(null) as string;
            IsUpToDate = false;
            // Get ports
            var _ports = NodeModel.PortManager.GetPorts();
            InputPorts = new ObservableCollection<InputPortViewModel>();
            OutputPorts = new ObservableCollection<OutputPortViewModel>();
            foreach (Main.Ports.Port p in _ports)
            {
                switch (p.PortType)
                {
                    case Main.Ports.PortType.Input:
                        InputPortViewModel inst = new InputPortViewModel(p, this);
                        inst.Port.ValueManager.UpToDateChanged += ValueManager_UpToDateChanged;
                        InputPorts.Add(inst);
                        break;
                    case Main.Ports.PortType.Output:
                        OutputPortViewModel inst2 = new OutputPortViewModel(p, this);
                        inst2.Port.ValueManager.UpToDateChanged += ValueManager_UpToDateChanged;
                        OutputPorts.Add(inst2);
                        break;
                }
            }
            Left = left;
            Top = top;
        }

        /// <summary>
        /// Raised when up to date property in port model changed
        /// </summary>
        void ValueManager_UpToDateChanged(object sender, EventArgs e)
        {
            if ((sender as Main.Ports.Port).Parent.PortManager.GetPorts().Count((x) => !x.ValueManager.UpToDate) == 0)
                this.IsUpToDate = true;
            else
                this.IsUpToDate = false;
        }

        /// <summary>
        /// Calcultae values in this node
        /// </summary>
        public void Update()
        {
            foreach (OutputPortViewModel port in OutputPorts)
            {
                port.UpdateVal();
            }
            this.IsUpToDate = true;
        }
    }
}

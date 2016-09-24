using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VisualProgrGUI.behavior;

namespace VisualProgrGUI.ViewModel.NodeTypes
{
    /// <summary>
    /// Type view model
    /// </summary>
    public class TypeViewModel : DependencyObject, IBundle, IDragable
    {
        public string Name { get; private set; }

        /// <summary>
        /// Only for leafs
        /// </summary>
        public Main.Nodes.NodeType NodeType { get; private set; }

        #region IsSelected
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(TypeViewModel), new PropertyMetadata(false));
        #endregion

        /// <summary>
        /// nodeType is only for leafs
        /// </summary>
        /// <param name="header"></param>
        /// <param name="nodeType"></param>
        public TypeViewModel(string name, Main.Nodes.NodeType nodeType)
        {
            NodeType = nodeType;
            Name = name;
        }

        #region Dragable
        public Type DataType
        {
            get { return typeof(TypeViewModel); }
        }

        public void Remove()
        {
            IsSelected = false;
        }
        #endregion
    }
}

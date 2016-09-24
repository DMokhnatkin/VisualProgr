using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Interactivity;
using VisualProgrGUI.behavior;
using System.Diagnostics;
using System.Windows.Input;
using VisualProgrGUI.ViewModel.Commands;

namespace VisualProgrGUI.ViewModel.NodeTypes
{
    public class NodeTypesListViewModel : DependencyObject
    {

        public ObservableCollection<IBundle> Elements { get; private set; }

        /// <summary>
        /// Node types model
        /// </summary>
        Main.Nodes.NodeTypesManager NodeTypes { get; set; }

        /// <summary>
        /// Reset node type selection
        /// </summary>
        public void ResetSelection()
        {
            Action<TreeViewItem> z = null;
            // Set all isSelected to false using DFS
            z = (x) => { x.IsSelected = false; foreach (TreeViewItem t in x.Items) z(t); };
            foreach(TreeViewItem t in Elements)
                z(t);
        }

        public NodeTypesListViewModel(Main.Nodes.NodeTypesManager _types)
        {
            NodeTypes = _types;
            Elements = new ObservableCollection<IBundle>();
            List<Main.Nodes.NodeType> types = _types.GetAll();
            foreach (Main.Nodes.NodeType t in types)
            {
                // If nodeType is in root, lets just create it
                if (t.Bundles.Count == 0)
                {
                    Elements.Add(new TypeViewModel(t.Name, t));
                    continue;
                }
                // Try find first group (if it isn't exists, create it) 
                TypeGroupViewModel cur = Elements.FirstOrDefault(x => x.Name == t.Bundles[0]) as TypeGroupViewModel;
                if (cur == null)
                {
                    cur = new TypeGroupViewModel(t.Bundles[0]);
                    Elements.Add(cur);
                }
                // Lets add other bundles
                foreach (string bundle in t.Bundles.Skip(1))
                {
                    // Create new TypeGroup if it isn't exists
                    var z = cur.Childs.FirstOrDefault(x => x.Name == bundle) as TypeGroupViewModel;
                    if (z == null)
                    {
                        z = new TypeGroupViewModel(bundle);
                        cur.Childs.Add(z);
                    }
                    cur = z;
                }
                // Lets add leaf(nodeType)
                cur.Childs.Add(new TypeViewModel(t.Name, t));
            }
        }
    }
}

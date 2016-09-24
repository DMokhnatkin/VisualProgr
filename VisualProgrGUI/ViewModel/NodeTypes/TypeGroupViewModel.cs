using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrGUI.ViewModel.NodeTypes
{
    /// <summary>
    /// Node type group view model
    /// </summary>
    public class TypeGroupViewModel : IBundle
    {
        /// <summary>
        /// Name of group
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Childs of this group
        /// </summary>
        public List<IBundle> Childs { get; private set; }

        public TypeGroupViewModel(string name)
        {
            Name = name;
            Childs = new List<IBundle>();
        }
    }
}

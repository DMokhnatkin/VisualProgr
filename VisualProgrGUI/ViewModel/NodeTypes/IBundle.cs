using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrGUI.ViewModel.NodeTypes
{
    /// <summary>
    /// Element in node types tree (bundles and leafs)
    /// </summary>
    public interface IBundle
    {
        string Name { get; }
    }
}

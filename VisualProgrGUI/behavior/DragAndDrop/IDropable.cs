using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrGUI.behavior
{
    /// <summary>
    /// Object which can be target in drag and drop operation
    /// </summary>
    public interface IDropable
    {
        /// <summary>
        /// Data type which can be droped in this element 
        /// </summary>
        Type DataType { get; }

        void Drop(DragEventArgs e);
    }
}

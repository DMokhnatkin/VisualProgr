using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrGUI.behavior
{
    /// <summary>
    /// Object which can be source in drag and drop operation
    /// </summary>
    public interface IDragable
    {
        /// <summary>
        /// Type of the data item
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// Remove data from source
        /// </summary>
        void Remove();
    }
}

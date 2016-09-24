using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchemeControlLib
{
    /// <summary>
    /// displaying nodes in the SchemeControl
    /// </summary>
    public class NodesControl : Selector
    {
        public NodesControl()
        {
            Focusable = false;
        }

        #region Private Methods

        /// <summary>
        /// Find the SchemeItem element that has the specified data context.
        /// Return null if no such NodeItem exists.
        /// </summary>
        internal SchemeItem FindAssociatedNodeItem(object itemDataContext)
        {
            return (SchemeItem)this.ItemContainerGenerator.ContainerFromItem(itemDataContext);
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item. 
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SchemeItem();
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container. 
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is SchemeItem;
        }

        #endregion Private Methods

        static NodesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodesControl), new FrameworkPropertyMetadata(typeof(NodesControl)));
        }
    }
}

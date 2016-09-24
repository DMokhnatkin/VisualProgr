using System;
using System.Windows;
using System.Windows.Interactivity;

namespace VisualProgrGUI.behavior
{

    public class FrameworkElementDropBehavior : Behavior<FrameworkElement>
    {
        Type dataType; // Data which can be dropped (just cache). We get it from IDropable

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.AllowDrop = true;
            this.AssociatedObject.DragEnter += new DragEventHandler(AssociatedObject_DragEnter);
            this.AssociatedObject.DragOver += new DragEventHandler(AssociatedObject_DragOver);
            this.AssociatedObject.Drop += new DragEventHandler(AssociatedObject_Drop);
        }

        void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            if (dataType != null)
            {
                if (e.Data.GetDataPresent(dataType))
                {
                    IDropable target = this.AssociatedObject.DataContext as IDropable;
                    target.Drop(e);

                    IDragable source = e.Data.GetData(dataType) as IDragable;
                    source.Remove();
                }
            }
        }

        void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {

        }

        /// <summary>
        /// Initialize drag and drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (this.dataType == null)
            {
                if (this.AssociatedObject.DataContext != null)
                {
                    IDropable dropObject = this.AssociatedObject.DataContext as IDropable;
                    if (dropObject != null)
                    {
                        this.dataType = dropObject.DataType;
                    }
                }
            }
            e.Handled = true;
        }

        /// <summary>
        /// Decide if data can be dropped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            if (dataType != null)
            {
                if (!e.Data.GetDataPresent(dataType))
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            e.Handled = true;
        }
    }
}

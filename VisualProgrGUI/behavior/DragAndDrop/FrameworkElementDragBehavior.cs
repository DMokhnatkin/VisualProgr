using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace VisualProgrGUI.behavior
{
    public class FrameworkElementDragBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// To verify when we do drag
        /// </summary>
        bool isMouseClicked = false;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_LeftButtonDown);
            this.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_LeftButtonUp);
            this.AssociatedObject.MouseLeave += new MouseEventHandler(AssociatedObject_MouseLeave);
        }

        void AssociatedObject_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = true;
        }

        void AssociatedObject_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseClicked = false;
        }

        void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isMouseClicked)
            {
                // Set item's data context as the data to be transfered
                IDragable dragObject = this.AssociatedObject.DataContext as IDragable;
                if (dragObject != null)
                {
                    DataObject data = new DataObject();
                    data.SetData(dragObject.DataType, dragObject);
                    // Start drag and drop operation
                    System.Windows.DragDrop.DoDragDrop(this.AssociatedObject, data, DragDropEffects.Copy);
                }
            }
            isMouseClicked = false;
        }
    }
}

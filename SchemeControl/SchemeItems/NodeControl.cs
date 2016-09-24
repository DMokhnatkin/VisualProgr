using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchemeControlLib
{
    /// <summary>
    /// Control for node
    /// </summary>
    [TemplatePart(Name="PART_Inputs", Type=typeof(ItemsControl))]
    [TemplatePart(Name="PART_Outputs", Type=typeof(ItemsControl))]
    public class NodeControl : Control
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(NodeControl));

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(NodeControl));   

        public static readonly DependencyProperty InputsSourceProperty =
            DependencyProperty.Register("InputsSource", typeof(IEnumerable), typeof(NodeControl), 
                                        new PropertyMetadata(new PropertyChangedCallback(InputsSourceProperty_Changed)));

        public static readonly DependencyProperty OutputsSourceProperty =
            DependencyProperty.Register("OutputsSource", typeof(IEnumerable), typeof(NodeControl),
                                        new PropertyMetadata(new PropertyChangedCallback(OutputsSourceProperty_Changed)));

        public static readonly DependencyPropertyKey InputsPropertyKey =
            DependencyProperty.RegisterReadOnly("Inputs", typeof(ObservableCollection<object>), typeof(NodeControl), 
                                                new PropertyMetadata());
        public static readonly DependencyProperty InputsProperty = InputsPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey OutputsPropertyKey =
            DependencyProperty.RegisterReadOnly("Outputs", typeof(ObservableCollection<object>), typeof(NodeControl),
                                        new PropertyMetadata());
        public static DependencyProperty OutputsProperty = OutputsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty InputTemplateProperty =
            DependencyProperty.Register("InputTemplate", typeof(DataTemplate), typeof(NodeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty OutputTemplateProperty =
            DependencyProperty.Register("OutputTemplate", typeof(DataTemplate), typeof(NodeControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ParentSchemeControlProperty = SchemeItem.ParentSchemeControlProperty.AddOwner(typeof(NodeControl));

        #endregion

        /// <summary>
        /// Header of node
        /// </summary>
        [Category("Common")]
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Brush of header background
        /// </summary>
        [Category("Brush")]
        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        /// <summary>
        /// Used to generate inputs
        /// </summary>
        [Category("Common")]
        public IEnumerable InputsSource
        {
            get { return (IEnumerable)GetValue(InputsSourceProperty); }
            set { SetValue(InputsSourceProperty, value); }
        }

        /// <summary>
        /// Used to generate outputs
        /// </summary>
        [Category("Common")]
        public IEnumerable OutputsSource
        {
            get { return (IEnumerable)GetValue(OutputsSourceProperty); }
            set { SetValue(OutputsSourceProperty, value); }
        }

        /// <summary>
        /// Inputs
        /// </summary>
        [Category("Common")]
        public ObservableCollection<object> Inputs
        {
            get { return (ObservableCollection<object>)GetValue(InputsProperty); }
            private set { SetValue(InputsPropertyKey, value); }
        }

        /// <summary>
        /// Outputs
        /// </summary>
        [Category("Common")]
        public ObservableCollection<object> Outputs
        {
            get { return (ObservableCollection<object>)GetValue(OutputsProperty); }
            private set { SetValue(OutputsPropertyKey, value); }
        }

        /// <summary>
        /// Input port template
        /// </summary>
        public DataTemplate InputTemplate
        {
            get { return (DataTemplate)GetValue(InputTemplateProperty); }
            set { SetValue(InputTemplateProperty, value); }
        }

        /// <summary>
        /// Output port template
        /// </summary>
        public DataTemplate OutputTemplate
        {
            get { return (DataTemplate)GetValue(OutputTemplateProperty); }
            set { SetValue(OutputTemplateProperty, value); }
        }

        /// <summary>
        /// Scheme control which contains this scheme item. (Set in default style)
        /// </summary>
        public SchemeControl ParentSchemeControl
        {
            get { return (SchemeControl)GetValue(SchemeItem.ParentSchemeControlProperty); }
            set { SetValue(SchemeItem.ParentSchemeControlProperty, value); }
        }

        #region Private Methods
        ItemsControl _inputs;
        ItemsControl _outputs;

        /// <summary>
        /// Raised when InputsSource property changed
        /// </summary>
        private static void InputsSourceProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged)
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= (sender as NodeControl).InputsSource_CollectionChanged;
            if (e.NewValue is INotifyCollectionChanged)
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += (sender as NodeControl).InputsSource_CollectionChanged;
            if (e.NewValue != null)
                // New collection can contains elements.
                // Raise InputsSource collection changed event to update
                (sender as NodeControl).InputsSource_CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                    new List<object>(e.NewValue as IEnumerable<object>)));
        }

        /// <summary>
        /// Raised when OutputsSource property changed
        /// </summary>
        private static void OutputsSourceProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged)
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= (sender as NodeControl).OutputsSource_CollectionChanged;
            if (e.NewValue is INotifyCollectionChanged)
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += (sender as NodeControl).OutputsSource_CollectionChanged;
            if (e.NewValue != null)
                // New collection can contains elements.
                // Raise OutputsSource collection changed event to update
                (sender as NodeControl).OutputsSource_CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                    new List<object>(e.NewValue as IEnumerable<object>)));
        }

        /// <summary>
        /// Raised when InputsSource collection changed
        /// We must create or remove ports
        /// </summary>
        void InputsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        // Add new ports
                        foreach (var t in e.NewItems)
                            Inputs.Add(t);
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        // Remove ports
                        foreach (var t in e.OldItems)
                            Inputs.Remove(t);
                        break;
                    }
            }
        }

        /// <summary>
        /// Raised when OutputsSource collection changed
        /// We must create or remove ports
        /// </summary>
        void OutputsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        // Add new ports
                        foreach (var t in e.NewItems)
                            Outputs.Add(t);
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        // Remove ports
                        foreach (var t in e.OldItems)
                            Outputs.Remove(t);
                        break;
                    }
            }
        }

        /// <summary>
        /// Raised when template applied
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _inputs = GetTemplateChild("PART_Inputs") as ItemsControl;
            _outputs = GetTemplateChild("PART_Outputs") as ItemsControl;
            ParentSchemeControl = FindAncestor.FindAncestorOfType(this, typeof(SchemeControl)) as SchemeControl;
        }


        #endregion

        static NodeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeControl), new FrameworkPropertyMetadata(typeof(NodeControl)));
        }

        public NodeControl()
        {
            Inputs = new ObservableCollection<object>();
            Outputs = new ObservableCollection<object>();
        }

    }
}

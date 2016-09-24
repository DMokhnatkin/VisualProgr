using System;
using System.Collections.Generic;
using System.Windows;

/// <summary>
/// Выход элемента
/// </summary>
public abstract class Output : Port
{
    static Output()
    {
        //Регистрируем обработчик события изменения свойтсва Value
        ValueProperty.OverrideMetadata(typeof(Output), new PropertyMetadata(new PropertyChangedCallback(ValuePropertyChanged)));
    }

    public Output(Node parent, string name) : base(parent, name)
    {}

    protected static void ValuePropertyChanged(System.Windows.DependencyObject sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue != e.OldValue)
            foreach (Port t in (sender as Port).Connected)
                t.ParentNode.NeedToBeUpdated = true;
    }
}

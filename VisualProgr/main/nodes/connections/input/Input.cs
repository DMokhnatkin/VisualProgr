using System;
using System.Xml;
using System.Windows.Data;
using System.Windows;

public abstract class Input : Port
{
    static Input()
    {
        //Регистрируем обработчик события изменения свойтсва Value в Input,
        ValueProperty.OverrideMetadata(typeof(Input), new PropertyMetadata(new PropertyChangedCallback(ValuePropertyChanged)));
    }

    public Input(Node parent, string name)
        : base(parent, name)
    {}

    protected static void ValuePropertyChanged(System.Windows.DependencyObject sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        (sender as Port).ParentNode.NeedToBeUpdated = true;
    }

    /// <summary>
    /// Добавить соединение к порту
    /// Для Input можно добавить только один связанный порт
    /// </summary>
    /// <param name="connect"></param>
    public override void AddConnection(Port connect)
    {
        if (Connected.Count != 0)
            throw new ArgumentOutOfRangeException("Попытка создать 2-ю связь для Input");
        base.AddConnection(connect);
        this.ParentNode.NeedToBeUpdated = true;
    }
}

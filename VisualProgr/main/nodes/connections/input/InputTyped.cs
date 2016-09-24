using System;
using System.Windows;
using System.Windows.Data;

/// <summary>
/// Вход элемента
/// </summary>
public class InputTyped<T> : Input
{
    static InputTyped()
    {
        //Регистрируем обработчик события изменения свойтсва Value в Input,
        ValueProperty.OverrideMetadata(typeof(InputTyped<T>), new PropertyMetadata(default(T), new PropertyChangedCallback(ValuePropertyChanged)));
    }

    public InputTyped(Node parent, string name)
        : base(parent, name)
    {}

    /// <summary>
    /// Добавить связь
    /// </summary>
    /// <param name="connection"></param>
    /// <exception cref="ArgumentException">Недопустимуя связь между нодами</exception>
    public override void AddConnection(Port connect)
    {
        if (!(connect is OutputTyped<T>))
            throw new ArgumentException("Попытка создать недопустимую связь между нодами");
        base.AddConnection(connect);
    }
}
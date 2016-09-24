using System;
using System.Windows;
using System.Windows.Data;

/// <summary>
/// Выход элемента
/// </summary>
public class OutputTyped<T> : Output
{
    public OutputTyped(Node parent, string name) : base(parent, name)
    {}

    public override void AddConnection(Port connect)
    {
        if (!(connect is InputTyped<T>))
            throw new ArgumentException("Несовместимость типов при создании связи");
        base.AddConnection(connect);
    }
}

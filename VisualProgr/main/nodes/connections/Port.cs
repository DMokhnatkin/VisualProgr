using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// Соединение
/// </summary>
public abstract class Port : DependencyObject
{
    #region Типы портов
    public enum Types { Input, Output, Setting };
    #endregion

    #region DependencyProperty Значение (Value)
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
        typeof(object), typeof(Port));

    public object Value
    {
        get { return GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
    #endregion

    #region DependencyProperty Соединен ли порт (IsNullConnected)
    public static readonly DependencyProperty IsNullConnectedProperty = DependencyProperty.Register("IsNullConnected",
    typeof(bool), typeof(Port), new PropertyMetadata(true));

    public bool IsNullConnected
    {
        get{return (bool)GetValue(IsNullConnectedProperty);}
        set { SetValue(IsNullConnectedProperty, value); }
    }
    #endregion

    #region _name : Имя соединения
    private string _name;
    /// <summary>
    /// Имя соединения
    /// </summary>
    public string Name
    {
        get { return _name; }
    }
    #endregion

    #region _parentNode : Родительский элемент управления
    private Node _parentNode;
    /// <summary>
    /// Родительский элемент управления
    /// </summary>
    public Node ParentNode
    {
        get { return _parentNode; }
    }
    #endregion

    #region Соединенные порты
    private List<Port> _connected = new List<Port>();

    /// <summary>
    /// Соединенные порты
    /// </summary>
    public ReadOnlyCollection<Port> Connected
    {
        get { return _connected.AsReadOnly(); }
    }

    /// <summary>
    /// Отсоединить порт с именем name
    /// </summary>
    /// <param name="name"></param>
    public virtual void DelConnection(Port port = null)
    {
        if (port == null)
            _connected.Clear();
        else
        {
            var t = _connected.Find(x => x == port);
            if (t != null)
                _connected.Remove(t);
        }
        if (Connected.Count == 0)
            IsNullConnected = true;
    }

    /// <summary>
    /// Присоединить порт
    /// </summary>
    /// <param name="port"></param>
    public virtual void AddConnection(Port port)
    {
        _connected.Add(port);
        if (Connected.Count != 0)
            IsNullConnected = false;
    }
    #endregion

    protected Port(Node parent, string name)
    {
        _parentNode = parent;
        _name = name;
    }
}

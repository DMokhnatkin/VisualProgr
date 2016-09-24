using System.Collections.Generic;
using System.Xml;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;

/// <summary>
/// Реализует ноду
/// </summary>
public abstract class Node : DependencyObject
{
    #region _name : Имя ноды
    string _name;
    /// <summary>
    /// Имя ноды
    /// </summary>
    public string Name
    {
        get { return _name; }
    }
    #endregion

    #region Порты
    NodePorts _ports = new NodePorts();
    /// <summary>
    /// Обеспечивает доступ к портам по имени
    /// </summary>
    public NodePorts Ports
    {
        get { return _ports; }
        protected set { _ports = value; }
    }
    #endregion

    #region Дополнительные данные для типа ноды (NodeTypeMetaData)
    /// <summary>
    /// Получает данные типа этой ноды
    /// </summary>
    public NodeTypeMetaData NodeTypeMetaData
    {
        get { return NodeTypes.GetNodeMetaData(this.GetType()); }
    }
    #endregion

    #region Добавление портов
    /// <summary>
    /// Добавить к ноде вход
    /// </summary>
    protected InputTyped<T> RegisterInput<T>(string name, T defaultValue)
    {
        InputTyped<T> t = new InputTyped<T>(this, name);
        Ports.AddPort(t, Port.Types.Input);
        return t;
    }

    /// <summary>
    /// Добавить к ноде выход
    /// </summary>
    protected OutputTyped<T> RegisterOutput<T>(string name)
    {
        OutputTyped<T> t = new OutputTyped<T>(this, name);
        Ports.AddPort(t, Port.Types.Output);
        return t;
    }

    /// <summary>
    /// Добавить к ноде настройку
    /// </summary>
    protected InputTyped<T> RegisterSetting<T>(string name, T defaultValue)
    {
        InputTyped<T> t = new InputTyped<T>(this, name);
        Ports.AddPort(t, Port.Types.Setting);
        return t;
    }
    #endregion

    #region Обновление ноды
    /// <summary>
    /// Обновлять автоматически при изменении NeedToBeUpdatedProperty
    /// </summary>
    private bool _autoUpdate = false;

    public bool AutoUpdate
    {
        get { return _autoUpdate; }
        set { _autoUpdate = value; }
    }

    private bool _blockUpdate = true;

    public bool BlockUpdate
    {
        get { return _blockUpdate; }
        set { _blockUpdate = value; }
    }

    public static readonly DependencyProperty NeedToBeUpdatedProperty = DependencyProperty.Register("NeedToBeUpdated",
        typeof(bool), typeof(Node), new PropertyMetadata(true, new PropertyChangedCallback(NeedToBeUpdatedPropertyChanged)));

    static void NeedToBeUpdatedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue)
        {
            foreach (Port t in (sender as Node).Ports.Outputs)
                foreach (Port k in t.Connected)
                        k.ParentNode.NeedToBeUpdated = true;
            if ((sender as Node).AutoUpdate)
                (sender as Node).Update();
        }
    }

    /// <summary>
    /// Нуждается ли нода в обновлении
    /// </summary>
    public bool NeedToBeUpdated
    {
        get { return (bool)GetValue(NeedToBeUpdatedProperty); }
        set { SetValue(NeedToBeUpdatedProperty, value); }
    }

    /// <summary>
    /// Обновить ноду
    /// </summary>
    public void Update()
    {
        if (BlockUpdate)
            return;
        foreach (Port t in Ports.Inputs)
            if (!t.IsNullConnected)
            {
                if (t.Connected[0].ParentNode.NeedToBeUpdated)
                    t.Connected[0].ParentNode.Update();
                t.Value = t.Connected[0].Value;
            }
        foreach (Port t in Ports.Settings)
            if (!t.IsNullConnected)
            {
                if (t.Connected[0].ParentNode.NeedToBeUpdated)
                    t.Connected[0].ParentNode.Update();
                t.Value = t.Connected[0].Value;
            }
        this.NeedToBeUpdated = false;
        Calculate();
    }
    #endregion

    public Node(string name)
    {
        _name = name;
    }

    /// <summary>
    /// Пересчитать значения(если значение на входе не определено
    /// то рекурсивно рассчитать предыдущие ноды)
    /// </summary>
    public abstract void Calculate();

    #region XML

    public  static Node ReadXml(XmlElement el)
    {
        string name = el.GetAttribute("name");
        string type = el.GetAttribute("type");
        Node node = (Node)Activator.CreateInstance(System.Type.GetType(type),name);
        return node;
    }
    #endregion
}

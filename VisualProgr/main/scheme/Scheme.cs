using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;
using System.Text;

/// <summary>
/// Реализует схему
/// </summary>
public class Scheme
{
    #region Текущая схема (Cur)
    /// <summary>
    /// Текущая сцена(только одна в проекте)
    /// </summary>
    private static Scheme _cur;

    public static Scheme Cur
    {
        get
        {
            if (_cur == null)
                _cur = new Scheme();
            return _cur;
        }
    }
    #endregion

    #region _nodes Ноды
    /// <summary>
    /// Ноды(доступ по имени)
    /// </summary>
    Dictionary<string, Node> _nodes;
    /// <summary>
    /// Добавить в схему ноду
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public void AddNode(Node node)
    {
        _nodes.Add(node.Name, node);
    }

    /// <summary>
    /// Удалить ноду
    /// </summary>
    /// <param name="node"></param>
    public void DeleteNode(Node node)
    {
        foreach (Port t in node.Ports.All)
            ConnectionManager.ClearConnections(t);
        _nodes.Remove(node.Name);
    }

    /// <summary>
    /// Получить ноду
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public Node GetNode(string name)
    {
        if (!_nodes.ContainsKey(name))
            throw new KeyNotFoundException("Can't find node in scheme");
        return _nodes[name];
    }
    /// <summary>
    /// Получить все ноды
    /// </summary>
    /// <returns></returns>
    public List<Node> GetNodes()
    {
        return new List<Node>(_nodes.Values);
    }
    /// <summary>
    /// Получить свободное имя для ноды
    /// </summary>
    /// <returns></returns>
    public string GetFreeName()
    {
        string s = "node";
        for (int i = 1; ; i++)
            if (!_nodes.ContainsKey(s + i.ToString()))
                return s + i.ToString();
    }
    #endregion

    public Scheme()
    {
        _nodes = new Dictionary<string, Node>();
    }

    /// <summary>
    /// Связяать ноды
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public void Connect(Port a, Port b)
    {
        ConnectionManager.Connect(a, b);
    }

    /// <summary>
    /// Очистить схему
    /// </summary>
    public void Clear()
    {
        _nodes.Clear();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

/// <summary>
/// Регистрирует и хранит типы нод с данными о них(типах)
/// </summary>
public class NodeTypes
{
    #region Ссылка на текущий экземпляр(он всего один в программе) (Cur) 
    private static NodeTypes _cur;
    public static NodeTypes Cur
    {
        get
        {
            if (_cur == null)
                _cur = new NodeTypes();
            return _cur;
        }
    }
    #endregion

    private Dictionary<Type, NodeTypeMetaData> _nodeTypes = new Dictionary<Type, NodeTypeMetaData>();

    private Dictionary<string, NodeTypeMetaData> _nodeTypesByName = new Dictionary<string, NodeTypeMetaData>();

    #region Типы нод, разделённые на группы (NodeTypesGroups)
    List<NodeTypeGroup> _nodeTypesGroups = new List<NodeTypeGroup>();

    /// <summary>
    /// Получить типы нод, разделенные по группам
    /// </summary>
    public static List<NodeTypeGroup> NodeTypeGroups
    {
        get { return Cur._nodeTypesGroups; }
    }
    #endregion

    /// <summary>
    /// Получить meta data для указанного типа ноды
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static NodeTypeMetaData GetNodeMetaData(Type type)
    {
        if (!Cur._nodeTypes.ContainsKey(type))
            throw new ArgumentException("Не найден тип ноды " + type.Name);
        return Cur._nodeTypes[type];
    }

    public static NodeTypeMetaData GetNodeMetaData(string name, string group)
    {
        if (!Cur._nodeTypesByName.ContainsKey(name + "-" + group))
            throw new ArgumentException("Не найден тип ноды " + name + "-" + group);
        return Cur._nodeTypesByName[name + "-" + group];
    }

    /// <summary>
    /// Зарегистрировать тип ноды
    /// </summary>
    /// <param name="nodeTypeName"></param>
    /// <param name="ports"></param>
    /// <param name="engine"></param>
    public static NodeTypeMetaData Register(NodeTypeMetaData metadata)
    {
        Cur._nodeTypes.Add(metadata.SystemType, metadata);
        Cur._nodeTypesByName.Add(metadata.Name + "-" + metadata.Group, metadata);
        if (NodeTypeGroups.Find(t => t.Name == metadata.Group) == null)
        {
            NodeTypeGroups.Add(new NodeTypeGroup(metadata.Group));
        }
        NodeTypeGroups.Find(t => t.Name == metadata.Group).AddNodeType(metadata);
        return metadata;
    }
}

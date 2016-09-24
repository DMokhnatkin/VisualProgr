using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Группа типов нод
/// </summary>
public class NodeTypeGroup
{
    #region Типы нод, которые содержит данная группа (NodeTypes)
    List<NodeTypeMetaData> _nodeTypes = new List<NodeTypeMetaData>();
    /// <summary>
    /// Типы нод, которые содержит данная группа
    /// </summary>
    public List<NodeTypeMetaData> NodeTypes
    {
        get { return _nodeTypes; }
    }
    
    public void AddNodeType(NodeTypeMetaData metaData)
    {
        _nodeTypes.Add(metaData);
    }
    #endregion

    #region Имя (Name)
    string _name;
    public string Name
    {
        get { return _name; }
    }
    #endregion

    public NodeTypeGroup(string name)
    {
        _name = name;
    }
}

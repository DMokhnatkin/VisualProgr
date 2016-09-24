using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

/// <summary>
/// Тип ноды
/// </summary>
public class NodeTypeMetaData
{
    #region Имя типа ноды (Name)
    string _nodeTypeName;

    public string Name
    {
        get { return _nodeTypeName; }
    }
    #endregion

    #region Класс который соответсвует этому типу
    Type _SystemType;
    /// <summary>
    /// Тип ноды
    /// </summary>
    public Type SystemType
    {
        get { return _SystemType; }
    }
    #endregion

    #region Иконка ноды (Icon)
    BitmapImage _icon;

    /// <summary>
    /// Иконка ноды
    /// </summary>
    public BitmapImage Icon
    {
        get { return _icon; }
    }
    #endregion

    #region Кисть топ бара
    Brush _topBarBrush;
    /// <summary>
    /// Цвет top bar
    /// </summary>
    public Brush TopBarBrush
    {
        get { return _topBarBrush; }
    }
    #endregion

    #region DependencyProperty; Группа (Group)
    string _nodeGroup;

    public string Group
    {
        get { return _nodeGroup; }
        private set { _nodeGroup =value; }
    }
    #endregion

    /// <summary>
    /// Информация о типе ноды
    /// </summary>
    /// <param name="nodeTypeName">Имя типа ноды</param>
    /// <param name="type">Тип ноды</param>
    /// <param name="icon">Иконка ноды</param>
    /// <param name="topBarBrush">Цвет top bar ноды</param>
    public NodeTypeMetaData(string nodeTypeName, Type type, ImageSource icon, string group, Brush topBarBrush)
    {
        Group = group;
        _nodeTypeName = nodeTypeName;
        _SystemType = type;
        _icon = (BitmapImage)icon;
        _topBarBrush = topBarBrush;
    }
}
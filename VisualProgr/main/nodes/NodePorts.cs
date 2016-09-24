using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

public class NodePorts
{
    #region Все порты
    public ReadOnlyCollection<Port> All
    {
        get { return _inputs.Union<Port>(_outputs).Union<Port>(_settings).ToList<Port>().AsReadOnly(); }
    }
    #endregion

    #region Входы Inputs
    //Для быстрого доступа к коллекции входов
    List<Input> _inputs = new List<Input>();
    /// <summary>
    /// Получить все входы для этой ноды
    /// </summary>
    public ReadOnlyCollection<Input> Inputs
    {
        get { return _inputs.AsReadOnly(); }
    }
    #endregion

    #region Выходы Outputs
    //Для быстрого доступа к коллекции выходов
    List<Output> _outputs = new List<Output>();
    /// <summary>
    /// Получить все выходы для этой ноды
    /// </summary>
    public ReadOnlyCollection<Output> Outputs
    {
        get { return _outputs.AsReadOnly(); }
    }
    #endregion

    #region Настройки Settings
    //Для быстрого доступа к коллекции настроеек
    List<Input> _settings = new List<Input>();
    /// <summary>
    /// Получить все настройки для этой ноды
    /// </summary>
    public ReadOnlyCollection<Input> Settings
    {
        get { return _settings.AsReadOnly(); }
    }
    #endregion

    #region Доступ к порту по имени
    //Для быстрого поиска по имени порта
    Dictionary<string,Port> _ports = new Dictionary<string,Port>();
    /// <summary>
    /// Получить порт
    /// </summary>
    /// <param name="name">Имя порта</param>
    public Port this[string name]
    {
        get 
        {
            if (!_ports.ContainsKey(name))
                throw new ArgumentException("Порт с именем " + name + " не найден");
            return _ports[name]; 
        }
    }
    #endregion

    /// <summary>
    /// Добавить порт
    /// </summary>
    /// <param name="type">Тип порта</param>
    public void AddPort(Port port, Port.Types type)
    {
        switch (type)
        {
            case Port.Types.Input:
                _inputs.Add((Input)port);
                break;
            case Port.Types.Output:
                _outputs.Add((Output)port);
                break;
            case Port.Types.Setting:
                _settings.Add((Input)port);
                break;
        }
        _ports.Add(port.Name, port);
    }
}

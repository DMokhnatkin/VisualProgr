using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConnectionManager
{
    /// <summary>
    /// Проверка на зацикливание
    /// </summary>
    /// <param name="cur">Нода в которой содержится Input, для которого создаеься связь</param>
    /// <param name="start">Нода в которой содержится Output, для которого создаеься связь</param>
    /// <param name="loop">Есть ли зацикливание</param>
    private static void LoopVerify(Node cur, Node start, ref bool loop)
    {
        if (loop)
            return;
        if (cur == start)
            loop = true;
        foreach (Output outp in cur.Ports.Outputs)
        {
            foreach (Input inp in outp.Connected)
            {
                LoopVerify(inp.ParentNode, start, ref loop);
            }
        }
    }

    /// <summary>
    /// Соединить два порта
    /// </summary>
    /// <param name="port1"></param>
    /// <param name="port2"></param>
    /// <exception cref="StackOverflowException">Зацикливание при создании связи</exception>
    public static void Connect(Port port1, Port port2)
    {
        bool loop = false;
        if (port1 is Input)
            LoopVerify(port1.ParentNode, port2.ParentNode, ref loop);
        else
            LoopVerify(port2.ParentNode, port1.ParentNode, ref loop);
        if (loop)
            throw new StackOverflowException("Зацикливание при создании связи");
        port1.AddConnection(port2);
        port2.AddConnection(port1);
    }

    /// <summary>
    /// Разьеденить 2 порта
    /// </summary>
    /// <param name="port1"></param>
    /// <param name="port2"></param>
    public static void Disconnect(Port port1, Port port2)
    {
        port1.DelConnection(port2);
    }

    /// <summary>
    /// Удалить все связи с этим портом
    /// </summary>
    /// <param name="port"></param>
    public static void ClearConnections(Port port)
    {
        foreach (Port t in port.Connected)
            t.DelConnection(port);
        port.DelConnection();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NodeBool_Const : Node
{
    #region Для удобного доступа
    OutputTyped<bool> outp;
    InputTyped<bool> val;
    #endregion

    public NodeBool_Const(string name)
        : base(name)
    {
        outp = RegisterOutput<bool>("outp");
        val = RegisterSetting<bool>("val", false);
        BlockUpdate = false;
    }

    /// <summary>
    /// Пересчитать значения на выходах элемента
    /// </summary>
    public override void Calculate()
    {
        outp.Value = val.Value;
    }
}

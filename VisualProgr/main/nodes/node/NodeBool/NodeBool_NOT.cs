using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class NodeBool_NOT : Node
{
    #region Для удобного доступа
    InputTyped<bool> in1;
    OutputTyped<bool> outp;
    #endregion

    public NodeBool_NOT(string name)
        : base(name)
    {
        in1 = RegisterInput<bool>("in1", false);
        outp = RegisterOutput<bool>("outp");
        BlockUpdate = false;
    }

    /// <summary>
    /// Пересчитать значения на выходах элемента
    /// </summary>
    public override void Calculate()
    {
        outp.Value = !(bool)in1.Value; 
    }
}

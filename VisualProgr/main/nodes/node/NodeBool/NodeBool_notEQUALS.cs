using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class NodeBool_notEQUALS : Node
{
    #region Для удобного доступа
    InputTyped<bool> in1;
    InputTyped<bool> in2;
    OutputTyped<bool> outp;
    #endregion

    public NodeBool_notEQUALS(string name)
        : base(name)
    {
        in1 = RegisterInput<bool>("in1", false);
        in2 = RegisterInput<bool>("in2", false);
        outp = RegisterOutput<bool>("outp");
        BlockUpdate = false;
    }

    /// <summary>
    /// Пересчитать значения на выходах элемента
    /// </summary>
    public override void Calculate()
    {
        outp.Value = !((bool)in1.Value == (bool)in2.Value); 
    }
}

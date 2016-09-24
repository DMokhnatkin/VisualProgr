using System.Collections.Generic;
/// <summary>
/// Реализует элемент OR
/// </summary>
public class NodeBool_OR : Node
{
    InputTyped<bool> _in1;
    InputTyped<bool> _in2;
    OutputTyped<bool> _outp;

    public NodeBool_OR(string name)
        : base(name)
    {
        _in1 = RegisterInput<bool>("in1", false);
        _in2 = RegisterInput<bool>("in2", false);
        _outp = RegisterOutput<bool>("outp");
        BlockUpdate = false;
    }

    /// <summary>
    /// Пересчитать значения на выходах элемента
    /// </summary>
    public override void Calculate()
    {
        _outp.Value = (bool)_in1.Value || (bool)_in2.Value;
    }
}

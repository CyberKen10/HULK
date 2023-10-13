using System;
using System.Collections.Generic;
namespace Hulk
{
class NumberNode : Node
{
    public Token Value { get; }

    public NumberNode(Token value)
    {
        Value = value;
    }

    public override object Evaluate()
    {
        if (int.TryParse(Value.Value, out int intValue))
        {
            return intValue;
        }
        else if (double.TryParse(Value.Value, out double doubleValue))
        {
            return doubleValue;
        }
        else
        {
            return Value.Value; // Devolver el valor como string si no es convertible a número.
        }
    }
    
}

}
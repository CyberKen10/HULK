namespace Hulk
{
class StringNode : Node
{
    public Token Value { get; }

    public StringNode(Token value)
    {
        Value = value;
    }

    public override object Evaluate()
    {
        return Value.Value; // Devuelve el valor de la cadena como está, sin cambios.
    }

    public override object EvaluateWithVariables(Dictionary<string, Node> variables)
    {
        return Value.Value; // También devuelve el valor de la cadena como está, sin cambios.
    }
}
}
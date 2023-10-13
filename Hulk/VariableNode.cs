using Hulk;
namespace Hulk
{
class VariableNode : Node
{
    public Token VariableName { get; }

    public VariableNode(Token variableName)
    {
        VariableName = variableName;
    }

    public override object Evaluate()
    {
        return VariableName;
    }

    public override object EvaluateWithVariables(Dictionary<string, Node> variables)
    {
        if (variables.ContainsKey(VariableName.Value))
        {
            // Devuelve el nodo correspondiente con los valores actualizados de las variables
            return variables[VariableName.Value];
        }
        else
        {
            throw new Exception($"Variable no definida: {VariableName.Value}");
        }
    }
}
}
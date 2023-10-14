namespace Hulk
{
class PrintNode : Node
{
    public Node Expression { get; }

    public PrintNode(Node expression)
    {
        Expression = expression;
    }

    public override object Evaluate()
{
    object result = Expression.Evaluate();
    return result.ToString();
}
    public override object EvaluateWithVariables(Dictionary<object, Node> variables)
    {
        object result = Expression.EvaluateWithVariables(variables);

        return result;
    }
}
}

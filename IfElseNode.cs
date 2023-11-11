namespace Hulk
{
class IfElseNode : Node
{
    public Node Condition { get; }
    public Node IfBranch { get; }
    public Node ElseBranch { get; }

    public IfElseNode(Node condition, Node ifBranch, Node elseBranch)
    {
        Condition = condition;
        IfBranch = ifBranch;
        ElseBranch = elseBranch;
    }

    public override object Evaluate()
    {
        var conditionResult = Condition.Evaluate();
        if (conditionResult is bool boolValue)
        {
            if (boolValue)
            {
                return IfBranch.Evaluate();
            }
            else
            {
                return ElseBranch.Evaluate();
            }
        }
        throw new InvalidOperationException("La condición del 'if' debe evaluar a un valor booleano.");
    }
    public override object EvaluateWithVariables(Dictionary<object, Node> variables)
    {
        var conditionResult = Condition.EvaluateWithVariables(variables);
        if (conditionResult is bool boolValue)
        {
            if (boolValue)
            {
                return IfBranch.EvaluateWithVariables(variables);
            }
            else
            {
                if (ElseBranch is NumberNode)
                {
                    return ElseBranch;
                }
                return ElseBranch.EvaluateWithVariables(variables);
            }
        }
        throw new InvalidOperationException("La condición del 'if' debe evaluar a un valor booleano.");
    }
}

}
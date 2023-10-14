namespace Hulk
{
class IfNode : Node
{
    public Node Condition { get; }
    public Node IfBranch { get; }

    public IfNode(Node condition, Node ifBranch)
    {
        Condition = condition;
        IfBranch = ifBranch;
    }

    public override object Evaluate()
    {
        var conditionResult = Condition.Evaluate();
        if (conditionResult is bool boolValue && boolValue)
        {
            return IfBranch.Evaluate();
        }
        return null; // Si la condición no se cumple, no se ejecuta la rama 'if'.
    }
    }    
}

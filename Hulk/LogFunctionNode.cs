namespace Hulk
{
class LogFunctionNode : Node
{
    public Node BaseExpression { get; }
    public Node NumberExpression { get; }

    public LogFunctionNode(Node baseExpression, Node numberExpression)
    {
        BaseExpression = baseExpression;
        NumberExpression = numberExpression;
    }

    public override object Evaluate()
    {
        double baseValue = Convert.ToDouble(BaseExpression.Evaluate());
        double number = Convert.ToDouble(NumberExpression.Evaluate());
        return Math.Log(number, baseValue);
    }

    public override object EvaluateWithVariables(Dictionary<object, Node> variables)
    {
        double baseValue = Convert.ToDouble(BaseExpression.EvaluateWithVariables(variables));
        double number = Convert.ToDouble(NumberExpression.EvaluateWithVariables(variables));
        return Math.Log(number, baseValue);
    }
}
}
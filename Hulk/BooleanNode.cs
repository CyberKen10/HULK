namespace Hulk
{
 class BooleanNode : Node
{
    private readonly Node left;
    private readonly Node right;
    private readonly Token operatorToken;

    public BooleanNode(Node left, Token operatorToken, Node right)
    {
        this.left = left;
        this.operatorToken = operatorToken;
        this.right = right;
    }

    public override object Evaluate()
    {
        // Evalúa las expresiones en el momento necesario
        double leftValue = Convert.ToDouble(left.Evaluate());
        double rightValue = Convert.ToDouble(right.Evaluate());

        switch (operatorToken.Value)
        {
            case ">":
                return leftValue > rightValue;
            case "<":
                return leftValue < rightValue;
            case "==":
                return leftValue == rightValue;
            case "!=":
                return leftValue != rightValue;
            case ">=":
                return leftValue >= rightValue;
            case "<=":
                return leftValue <= rightValue;
            default:
                throw new Exception($"Operador no válido: {operatorToken.Value}");
        }
    }
    public override object EvaluateWithVariables(Dictionary<string, Node> variables)
    {
        Node Left= left.EvaluateWithVariables(variables) as Node;
        Node Right= right.EvaluateWithVariables(variables) as Node;
        // Evalúa las expresiones en el momento necesario
        double leftValue = Convert.ToDouble(Left.Evaluate());
        double rightValue = Convert.ToDouble(Right.Evaluate());

        switch (operatorToken.Value)
        {
            case ">":
                return leftValue > rightValue;
            case "<":
                return leftValue < rightValue;
            case "==":
                return leftValue == rightValue;
            case "!=":
                return leftValue != rightValue;
            case ">=":
                return leftValue >= rightValue;
            case "<=":
                return leftValue <= rightValue;
        
            default:
                throw new Exception($"Operador no válido: {operatorToken.Value}");
        }
    }
}
}


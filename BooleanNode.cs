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
    public override object EvaluateWithVariables(Dictionary<object, Node> variables)
    {
        Node Left ;
        Node Right;
    if (left is NumberNode|| left is StringNode)
    {
        Left=left;
    }
    else
    {
        Left = left.EvaluateWithVariables(variables)as Node;
    }
    if (right is NumberNode|| right is StringNode)
    {
        Right=right;
    }
    else
    {
        Right = right.EvaluateWithVariables(variables)as Node;
    }
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


namespace Hulk
{
class UnaryOperationNode : Node
{
    public string Operator { get; }
    public Node Right { get; }

    public UnaryOperationNode(string oper, Node right)
    {
        Operator = oper;
        Right = right;
    }

    public override object Evaluate()
{
    object rightValue = Right.Evaluate();

    switch (Operator)
    {
        case "-":
            if (rightValue is double)
                return -Convert.ToDouble(rightValue);
            else
                return -Convert.ToInt32(rightValue);
        default:
            throw new InvalidOperationException("Operador unario desconocido");
    }
}
}
}
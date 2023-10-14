namespace Hulk
{
class MathFunctionNode : Node
{
    public string FunctionName { get; }
    public Node Argument { get; }

    public MathFunctionNode(string functionName, Node argument)
    {
        FunctionName = functionName;
        Argument = argument;
    }

    public override object Evaluate()
    {
        // Agregar lógica para evaluar las funciones aquí
        if (FunctionName == "cos")
        {
            double angle = Convert.ToDouble(Argument.Evaluate());
            return Math.Cos(angle);
        }
        else if (FunctionName == "sen")
        {
            double angle = Convert.ToDouble(Argument.Evaluate());
            return Math.Sin(angle);
        }
        else if (FunctionName == "log")
        {
            double value = Convert.ToDouble(Argument.Evaluate());
            return Math.Log(value);
        }
        else
        {
            throw new Exception($"Función desconocida: {FunctionName}");
        }
    }
    public override object EvaluateWithVariables(Dictionary<object, Node> variables)
    {   
        // Agregar lógica para evaluar las funciones aquí
        if (FunctionName == "cos")
        {
            double angle = Convert.ToDouble(Argument.EvaluateWithVariables(variables));
            return Math.Cos(angle);
        }   
        else if (FunctionName == "sen")
        {
            double angle = Convert.ToDouble(Argument.EvaluateWithVariables(variables));
            return Math.Sin(angle);
        }
        else if (FunctionName == "log")
        {
            double value = Convert.ToDouble(Argument.EvaluateWithVariables(variables));
            return Math.Log(value);
        }
        else
        {
            throw new Exception($"Función desconocida: {FunctionName}");
        }
    }
}
}
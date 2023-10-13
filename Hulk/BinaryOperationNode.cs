using System;
using System.Collections.Generic;
namespace Hulk
{
class BinaryOperationNode : Node
{
    public Node Left { get; }
    public Token Operator { get; }
    public Node Right { get; }

    public BinaryOperationNode(Node left, Token oper, Node right)
    {
        Left = left;
        Operator = oper;
        Right = right;
    }

    public override object Evaluate()
{
    if (AreTypesIncompatible(Left, Right, Operator.Value))
    {
        throw new Exception($"SEMANTIC ERROR: No se puede realizar la operación {Operator.Value} con los tipos de datos correspondientes");
    }
    object leftValue = Left.Evaluate();
    object rightValue = Right.Evaluate();
    if (AreTypesIncompatible(leftValue, rightValue, Operator.Value))
    {
        throw new Exception($"SEMANTIC ERROR: No se puede realizar la operación '{Operator.Value}' con los tipos de datos correspondientes");
    }

    switch (Operator.Type)
    {
        case TokenType.Operator:
            switch (Operator.Value)
            {
                case "+":
            if (leftValue is double || rightValue is double)
            {
                // Al menos uno de los valores es double, realiza la suma como double
                return Convert.ToDouble(leftValue) + Convert.ToDouble(rightValue);
            }
            else
            {
                // Ambos valores son enteros, realiza la suma como entero
                return Convert.ToInt32(leftValue) + Convert.ToInt32(rightValue);
            }
        case "-":
            if (leftValue is double || rightValue is double)
            {
                // Al menos uno de los valores es double, realiza la resta como double
                return Convert.ToDouble(leftValue) - Convert.ToDouble(rightValue);
            }
            else
            {
                // Ambos valores son enteros, realiza la resta como entero
                return Convert.ToInt32(leftValue) - Convert.ToInt32(rightValue);
            }
        case "%":
                // Operador de módulo (%)
            if (leftValue is double || rightValue is double)
            {
                return Convert.ToDouble(leftValue) % Convert.ToDouble(rightValue);
            }
            else
            {
                return Convert.ToInt32(leftValue) % Convert.ToInt32(rightValue);
            }
        case "*":
            if (leftValue is double || rightValue is double)
            {
                // Al menos uno de los valores es double, realiza la multiplicación como double
                return Convert.ToDouble(leftValue) * Convert.ToDouble(rightValue);
            }
            else
            {
                // Ambos valores son enteros, realiza la multiplicación como entero
                return Convert.ToInt32(leftValue) * Convert.ToInt32(rightValue);
            }
        case "/":
            // Siempre realiza la división como double para evitar divisiones enteras truncadas
            return Convert.ToDouble(leftValue) / Convert.ToDouble(rightValue);
        case "^":
            if (leftValue is double || rightValue is double)
            {
                // Al menos uno de los valores es double, realiza la potenciación como double
                return Math.Pow(Convert.ToDouble(leftValue), Convert.ToDouble(rightValue));
            }
            else
            {
                // Ambos valores son enteros, realiza la potenciación como entero
                return (int)Math.Pow(Convert.ToDouble(leftValue), Convert.ToDouble(rightValue));
            }
        case "@":
            // Concatenación de cadenas
            return leftValue.ToString() + " " + rightValue.ToString();
        default:
            throw new InvalidOperationException("Operador desconocido");
            }
        case TokenType.UnaryOperator:
            switch (Operator.Value)
            {
                case "-":
                     if (leftValue is double || rightValue is double)
                {
                    // Al menos uno de los valores es double, realiza la resta como double
                    return Convert.ToDouble(leftValue) - Convert.ToDouble(rightValue);
                }
                else
                {
                    // Ambos valores son enteros, realiza la resta como entero
                    return Convert.ToInt32(leftValue) - Convert.ToInt32(rightValue);
                }
                default:
                    throw new InvalidOperationException("Operador unario desconocido");
            }
        default:
            throw new InvalidOperationException("Token de operador esperado");

}
    
}
    public override Node EvaluateWithVariables(Dictionary<string, Node> variables)
{
    Node? left = Left.EvaluateWithVariables(variables)as Node;
    Node? right = Right.EvaluateWithVariables(variables)as Node;
    

    // En lugar de evaluar los valores aquí, simplemente crea un nuevo BinaryOperationNode
    // con los nodos izquierdo y derecho actualizados con los resultados de la evaluación.
    return new BinaryOperationNode(left, Operator, right);
}
private bool AreTypesIncompatible(object leftValue, object rightValue, string operatorValue)
{
    if (operatorValue == "+")
    {
        return !((leftValue is NumberNode || leftValue is StringNode) &&
                 (rightValue is NumberNode || rightValue is StringNode));
    }
    else if (operatorValue == "-")
    {
        return !(leftValue is NumberNode || leftValue is StringNode) ||
               !(rightValue is NumberNode || rightValue is StringNode);
    }
    else if (operatorValue == "*")
    {
        return !(leftValue is NumberNode || leftValue is StringNode) ||
               !(rightValue is NumberNode || rightValue is StringNode);
    }
    else if (operatorValue == "/")
    {
        return !(leftValue is NumberNode || leftValue is StringNode) ||
               !(rightValue is NumberNode || rightValue is StringNode);
    }
    // Añadir más comprobaciones para otros operadores según sus compatibilidades
    else
    {
        return false; // No se considera incompatible en otros casos
    }
}
}
}



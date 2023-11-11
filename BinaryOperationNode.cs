using System;
using System.Collections.Generic;
namespace Hulk
{
class BinaryOperationNode : Node
{
    public Node Left { get;set; }
    public Token Operator { get;set; }
    public Node Right { get;set; }

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
    public override object EvaluateWithVariables(Dictionary<object, Node> variables)
{   
    Node left ;
    Node right;
    if (Left is NumberNode|| Left is StringNode)
    {
        left=Left;
    }
    else
    {
        if (Left.EvaluateWithVariables(variables) is NumberNode LeFt )
        {
            left=LeFt;
        }
        else
        {
            left = Left.EvaluateWithVariables(variables)as Node;
        }
        
    }
    if (Right is NumberNode|| Right is StringNode)
    {
        right=Right;
    }
    else
    {
        if (Right.EvaluateWithVariables(variables) is Node RighT)
        {
            right=RighT;
        }
        else
        {
            right = Right.EvaluateWithVariables(variables)as Node;
        }
        
    }
   
     if (left != null && right != null)
    {
        // Ambos lados son instancias de Node, así que los evaluamos
        return new BinaryOperationNode(left, Operator, right);
    }
    else
    {
        // Al menos uno de los lados no es un Node, simplemente devolvemos la operación sin evaluar
        return new BinaryOperationNode(left ?? Left, Operator, right ?? Right);
    }

    // En lugar de evaluar los valores aquí, simplemente crea un nuevo BinaryOperationNode
    // con los nodos izquierdo y derecho actualizados con los resultados de la evaluación.
    return new BinaryOperationNode(left, Operator, right);
}
private bool AreTypesIncompatible(object leftValue, object rightValue, string operatorValue)
{
    if (operatorValue == "+")
    {
        if (leftValue is BinaryOperationNode && rightValue is NumberNode||rightValue is BinaryOperationNode&& leftValue is NumberNode)
        {
            return false;
        }
        
        if (leftValue is BinaryOperationNode && rightValue is BinaryOperationNode)
        {
            return false;
        }
        return !((IsNumericType(leftValue) || leftValue is string || leftValue is bool || leftValue is StringNode || leftValue is BooleanNode) &&
                 (IsNumericType(rightValue) || rightValue is string || rightValue is bool || rightValue is StringNode || rightValue is BooleanNode));
    }
    else if (operatorValue == "-")
    {
        if (leftValue is BinaryOperationNode && rightValue is NumberNode||rightValue is BinaryOperationNode&& leftValue is NumberNode)
        {
            return false;
        }
         if (leftValue is BinaryOperationNode && rightValue is BinaryOperationNode)
        {
            return false;
        }
        return !(IsNumericType(leftValue) || leftValue is string || leftValue is bool || leftValue is StringNode || leftValue is BooleanNode) ||
               !(IsNumericType(rightValue) || rightValue is string || rightValue is bool || rightValue is StringNode || rightValue is BooleanNode);
    }
    else if (operatorValue == "*")
    {
         if (leftValue is BinaryOperationNode && rightValue is NumberNode||rightValue is BinaryOperationNode&& leftValue is NumberNode)
        {
            return false;
        }
        
        if (leftValue is BinaryOperationNode && rightValue is BinaryOperationNode)
        {
            return false;
        }
        return !(IsNumericType(leftValue) || leftValue is string || leftValue is bool || leftValue is StringNode || leftValue is BooleanNode) ||
               !(IsNumericType(rightValue) || rightValue is string || rightValue is bool || rightValue is StringNode || rightValue is BooleanNode);
    }
    else if (operatorValue == "/")
    {
         if (leftValue is BinaryOperationNode && rightValue is NumberNode||rightValue is BinaryOperationNode&& leftValue is NumberNode)
        {
            return false;
        }
        
        if (leftValue is BinaryOperationNode && rightValue is BinaryOperationNode)
        {
            return false;
        }
        return !(IsNumericType(leftValue) || leftValue is string || leftValue is bool || leftValue is StringNode || leftValue is BooleanNode) ||
               !(IsNumericType(rightValue) || rightValue is string || rightValue is bool || rightValue is StringNode || rightValue is BooleanNode);
    }
    // Agregar más comprobaciones para otros operadores según sus compatibilidades
    else
    {
        return false; // No se considera incompatible en otros casos
    }
}

private bool IsNumericType(object value)
{
    return value is int || value is double || value is NumberNode;
}
}
}




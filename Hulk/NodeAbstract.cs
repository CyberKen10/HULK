using System;
using System.Collections.Generic;
namespace Hulk
{
abstract class Node
{
    public abstract object Evaluate();
    public virtual object EvaluateWithVariables(Dictionary<object, Node> variables)
    {
    return Evaluate();
    }

    }
}
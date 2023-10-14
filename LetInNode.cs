
namespace Hulk
{
 class LetInNode : Node
{
    public Dictionary<object, Node> DeclaredVariables { get; }
    public Node Statement { get; }


    public LetInNode(Dictionary<object, Node> declaredVariables, Node statement)
    {
        DeclaredVariables = declaredVariables;
        Statement = statement;
    }

    public override object Evaluate()
    {
        // Crea un nuevo diccionario para las variables evaluadas
       Dictionary<object, object> evaluatedVariables = new Dictionary<object, object>();


        // Evalúa cada expresión de variable y agrégala al nuevo diccionario
        foreach (var kvp in DeclaredVariables)
        {
            evaluatedVariables[kvp.Key] = kvp.Value.Evaluate();
        }

        return Statement.Evaluate();
    }
}
}
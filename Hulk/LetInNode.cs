
namespace Hulk
{
 class LetInNode : Node
{
    public Dictionary<string, Node> DeclaredVariables { get; }
    public Node Statement { get; }


    public LetInNode(Dictionary<string, Node> declaredVariables, Node statement)
    {
        DeclaredVariables = declaredVariables;
        Statement = statement;
    }

    public override object Evaluate()
    {
        // Crea un nuevo diccionario para las variables evaluadas
        Dictionary<string, object> evaluatedVariables = new Dictionary<string, object>();

        // Evalúa cada expresión de variable y agrégala al nuevo diccionario
        foreach (var kvp in DeclaredVariables)
        {
            evaluatedVariables[kvp.Key] = kvp.Value.Evaluate();
        }

        return Statement.Evaluate();
    }
}
}
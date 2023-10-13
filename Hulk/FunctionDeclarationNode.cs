namespace Hulk
{
    class FunctionDeclarationNode : Node
{
    public string Name { get; }
    public List<string> Parameters { get; }
    public Node Body { get; }

    public FunctionDeclarationNode(string name, List<string> parameters, Node body)
    {
        Name = name;
        Parameters = parameters;
        Body = body;
    }

    public override object Evaluate()
    {
        // No evaluamos la función en este momento; se evaluará en su llamada
        return this;
    }
     public override object EvaluateWithVariables(Dictionary<string, Node> variables)
    {
        // En este método, debes evaluar el cuerpo de la función en el contexto de las variables proporcionadas.
        // Crea un nuevo diccionario para las variables locales de la función
        Dictionary<string, Node> localVariables = new Dictionary<string, Node>();

        // Agrega los parámetros al diccionario local
        for (int i = 0; i < Parameters.Count; i++)
        {
            localVariables[Parameters[i]] = variables[Parameters[i]];
        }

        // Evalúa el cuerpo de la función en el contexto local
        return Body.EvaluateWithVariables(localVariables);
    }
}
}
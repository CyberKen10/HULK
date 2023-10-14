namespace Hulk
{
    class FunctionDeclarationNode : Node
{
    public string Name { get;set; }
    public List<object> Parameters { get;set; }
    public Node Body { get;set; }

    public FunctionDeclarationNode(string name, List<object> parameters, Node body)
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
     public override object EvaluateWithVariables(Dictionary<object, Node> variables)
    {
        // En este método, debes evaluar el cuerpo de la función en el contexto de las variables proporcionadas.
        // Crea un nuevo diccionario para las variables locales de la función
        Dictionary<object, Node> localVariables = new Dictionary<object, Node>();

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
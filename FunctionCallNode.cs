using System.Reflection.Metadata.Ecma335;

namespace Hulk
{
    class FunctionCallNode : Node
    {
        public string FunctionName { get; }
        public List<Node> Arguments { get; }
        private Dictionary<string, FunctionDeclarationNode> userDefinedFunctions;
        private List<object> variables;
        private Stack<Dictionary<object, Node>> functionCallStack;


        public FunctionCallNode(string functionName, List<Node> arguments,Dictionary<string, FunctionDeclarationNode> userDefinedFunctions,Stack<Dictionary<object, Node>> functionCallStack)
        {
            FunctionName = functionName;
            Arguments = arguments;
            this.userDefinedFunctions = userDefinedFunctions;
            this.variables = userDefinedFunctions[FunctionName].Parameters;
            this.functionCallStack= functionCallStack;
        }

        public override object Evaluate()
        {
                if (userDefinedFunctions.ContainsKey(FunctionName))
                {
                    // Encuentra la función definida por el usuario
                    FunctionDeclarationNode functionDeclaration = userDefinedFunctions[FunctionName];

                    // Verifica si el número de argumentos coincide con la declaración de la función
                    if (functionDeclaration.Parameters.Count != Arguments.Count)
                    {
                        throw new Exception($"Número incorrecto de argumentos para la función '{FunctionName}'");
                    }

                    // Crea un nuevo diccionario para las variables locales de la función
                    Dictionary<object, Node> localVariables = new Dictionary<object, Node>();

                    // Agrega los parámetros al diccionario local
                    for (int i = 0; i < functionDeclaration.Parameters.Count; i++)
                    {
                        localVariables[functionDeclaration.Parameters[i]] = Arguments[i];
                    }

                    // Push el diccionario de variables locales a la pila
                    functionCallStack.Push(localVariables);

                    // Evalúa la función con las variables locales
                    object result = functionDeclaration.Body.EvaluateWithVariables(functionCallStack.Peek());

                    // Pop el diccionario de variables locales de la pila
                    functionCallStack.Pop();
                if (result is Node resultNode)
                {
                    return resultNode.Evaluate() ;
                }
                else
                {
                    return result;
                }
                }
                else
                {
                    throw new Exception($"Función no definida: {FunctionName}");
                }
            }
            public override object EvaluateWithVariables(Dictionary<object, Node> variables)
{
    if (userDefinedFunctions.ContainsKey(FunctionName))
    {
        // Encuentra la función definida por el usuario
        FunctionDeclarationNode functionDeclaration = userDefinedFunctions[FunctionName];

        // Verifica si el número de argumentos coincide con la declaración de la función
        if (functionDeclaration.Parameters.Count != Arguments.Count)
        {
            throw new Exception($"Número incorrecto de argumentos para la función '{FunctionName}'");
        }

        // Crea un nuevo diccionario para las variables locales de la función
        Dictionary<object, Node> localVariables = new Dictionary<object, Node>();

        // Agrega los parámetros al diccionario local
        for (int i = 0; i < functionDeclaration.Parameters.Count; i++)
        {
            localVariables[functionDeclaration.Parameters[i]] = Arguments[i];
        }

        // Push el diccionario de variables locales a la pila
        functionCallStack.Push(localVariables);

        // Evalúa la función con las variables locales
        object result = functionDeclaration.Body.EvaluateWithVariables(functionCallStack.Peek());

        // Pop el diccionario de variables locales de la pila
        functionCallStack.Pop();

        if (result is Node resultNode)
        {
            return resultNode.Evaluate();
        }
        else
        {
            return result;
        }
    }
    else
    {
        throw new Exception($"Función no definida: {FunctionName}");
    }
}
        }
    }

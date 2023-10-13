using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
namespace Hulk
{
    
 class Parser
    {
        private Lexer lexer;
        private Token[] tokens;
        private int currentTokenIndex;
        private Dictionary<string,Node> variables = new Dictionary<string,Node>();
        private Dictionary<string, FunctionDeclarationNode> userDefinedFunctions = new Dictionary<string, FunctionDeclarationNode>();
        List<string> parameters = new List<string>();
        private Stack<Dictionary<string, Node>> functionCallStack = new Stack<Dictionary<string, Node>>();
        public Parser(Lexer lexer,Dictionary<string, FunctionDeclarationNode> userDefinedFunctions)
        {
            this.lexer = lexer;
            this.tokens = lexer.Tokenize();
            this.currentTokenIndex = 0;
            this.userDefinedFunctions=userDefinedFunctions;
        }

        public Node Parse()
        {
            return ParseStatement();
        }
        private Node ParseStatements()
        {
        Node result = null;

        while (currentTokenIndex < tokens.Length)
        {
            result = ParseStatement();
        }

        return result;
        }

        private Node ParseStatement()
        {
            if (Match(TokenType.Print))
            {
                Consume(); // Consume 'print'
                Expect(TokenType.Parenthesis, "(");
                Consume(); // Consume '('
                Node expression = ParseExpression();
                Expect(TokenType.Parenthesis, ")");
                Consume(); // Consume '
                return new PrintNode(expression);
            }
            else if (Match(TokenType.FunctionDeclaration))
            {
                return ParseFunctionDeclaration();
            }
            else if (Match(TokenType.If))
            {
                Consume(); // Consume 'if'
                Node condition = ParseParenthesizedExpression() as BooleanNode;
                if (!(condition is BooleanNode))
                {
                    throw new Exception("Error de sintaxis: La condición del 'if' debe evaluar a un valor booleano.");
                }
                Node ifBranch = ParseStatement();
                if (Match(TokenType.Else))
                {
                    Consume(); // Consume 'else'
                    Node elseBranch = ParseStatement();
                    return new IfElseNode(condition, ifBranch, elseBranch);
                }
                else
                {
                    return new IfNode(condition, ifBranch);
                }
            }
            else if (Match(TokenType.Let))
            {
                 return ParseLetIn();
            }
            if (Match(TokenType.Number))
            {
                return new NumberNode(Consume());
            }
    
            else if (Match(TokenType.String))
            {
                return new StringNode(Consume());
            }           
            else
            {
                return ParseExpression();
            }
        }
    private Node ParseFunctionDeclaration()
{
    Consume(); // Consume 'function'
    Expect(TokenType.Identifier); // Espera un identificador para el nombre de la función
    string functionName = Consume().Value;
    Expect(TokenType.Parenthesis, "(");
    Consume(); // Consume '('

    // Parsea los parámetros de la función
    while (!Match(TokenType.Parenthesis, ")"))
    {
        Expect(TokenType.Identifier); // Espera un identificador para el parámetro
        parameters.Add(Consume().Value);

        if (Match(TokenType.Coma))
        {
            Consume(); // Consume ','
        }
    }

    Expect(TokenType.Parenthesis, ")");
    Consume(); // Consume ')'

    Expect(TokenType.Arrow, "=>"); // Espera '=>'
    Consume(); // Consume '=>'

    // Parsea la expresión de la función
    Node body = ParseExpression();

    // Crea un nodo FunctionDeclarationNode y agrega la función al diccionario userDefinedFunctions
    FunctionDeclarationNode functionNode = new FunctionDeclarationNode(functionName, parameters, body);
    userDefinedFunctions[functionName] = functionNode;

    // Espera un punto y coma al final de la declaración de función
    Expect(TokenType.PuntoYComa, ";");
    Consume(); // Consume el punto y coma

    return functionNode; // Devuelve el nodo de declaración de función
}
    private Node ParseExpression()
{
    Node left = ParseTerm();
    while(true){
    while (Match(TokenType.Operator, "*") || Match(TokenType.Operator, "/") || Match(TokenType.Operator, "^")||Match(TokenType.Operator,"%"))
        {
            Token operatorToken = Consume();
            Node right = ParseFactor();
            left = new BinaryOperationNode(left, operatorToken, right);
        }
    while (Match(TokenType.Operator, ">") || Match(TokenType.Operator, "<") || Match(TokenType.Operator, "==") ||
           Match(TokenType.Operator, "!=") || Match(TokenType.Operator, ">=") || Match(TokenType.Operator, "<=") ||
           Match(TokenType.Operator, "@"))
    {
        Token operatorToken = Consume();
        Node right = ParseTerm();

        if (operatorToken.Type == TokenType.Operator && operatorToken.Value == "@")
        {
            left = new BinaryOperationNode(left, operatorToken, right);
        }
        else
        {
            left = new BooleanNode(left, operatorToken, right);
        }
    }
     if (Match(TokenType.Operator,"@"))
    {
        Token operatorToken = Consume();
        Node right= ParseTerm();
        left = new BinaryOperationNode(left, new Token(TokenType.Operator, "@"), right);
    }
    else if (Match(TokenType.Let))
        {
        return ParseLetIn();
        }
    else if (Match(TokenType.Identifier))
    {
       Token FunctionName = Consume();
            string functionName = FunctionName.Value;

            if (Match(TokenType.Parenthesis, "("))
            {
                Consume(); // Consume '('
                List<Node> arguments = new List<Node>();

                while (!Match(TokenType.Parenthesis, ")"))
                {
                    Node argument = ParseExpression();
                    arguments.Add(argument);

                    if (Match(TokenType.Coma))
                    {
                        Consume(); // Consume ','
                    }
                }

                Expect(TokenType.Parenthesis, ")");
                Consume(); // Consume ')'

                if (userDefinedFunctions.ContainsKey(functionName))
                {
                    // Encuentra la función definida por el usuario
                    FunctionDeclarationNode functionDeclaration = userDefinedFunctions[functionName];

                    // Verifica si el número de argumentos coincide con la declaración de la función
                    if (functionDeclaration.Parameters.Count != arguments.Count)
                    {
                        throw new Exception($"Número incorrecto de argumentos para la función '{functionName}'");
                    }

                    // Crea un nuevo diccionario para las variables locales de la función
                    Dictionary<string, Node> localVariables = new Dictionary<string, Node>();

                    // Agrega los parámetros al diccionario local
                    for (int i = 0; i < functionDeclaration.Parameters.Count; i++)
                    {
                        localVariables[functionDeclaration.Parameters[i]] = arguments[i];
                    }

                    // Agrega las variables globales al diccionario local
                    foreach (var variable in variables)
                    {
                        localVariables[variable.Key] = variable.Value;
                    }

                    // Push el diccionario de variables locales a la pila
                    functionCallStack.Push(localVariables);

                    // Evalúa la función con las variables locales
                    Node result = functionDeclaration.Body.EvaluateWithVariables(localVariables) as Node;

                    // Pop el diccionario de variables locales de la pila
                    functionCallStack.Pop();

                    return result;
                }
                else
                {
                    throw new Exception($"Función no definida: {functionName}");
                }
            }
    }
    else if (Match(TokenType.Operator, "+") || Match(TokenType.UnaryOperator, "-"))
    {
        while (Match(TokenType.Operator, "+") || Match(TokenType.UnaryOperator, "-"))
        {
            Token operatorToken = Consume();
            Node right = ParseTerm();
            left = new BinaryOperationNode(left, operatorToken, right);
        }
    }
    if (Match(TokenType.Parenthesis, "("))
    {
        left = ParseParenthesizedExpression(); // Usa la nueva función para parsear la expresión entre paréntesis
    }
    else
    {
        break;
    }
    }
    return left;
}
    private Node ParseLetIn()
{
     Consume(); // Consume 'let'
    Dictionary<string, Node> declaredVariables = new Dictionary<string, Node>();
    while (true)
    {
        string variableName = Consume().Value; // Consume el nombre de la variable
        Expect(TokenType.Operator, "=");
        Consume(); // Consume '='
        Node expression = ParseExpression();
        declaredVariables[variableName] = expression;

        if (Match(TokenType.Coma))
        {
            Consume(); // Consume ','
        }
        else
        {
            break; // Sal del bucle si no hay más variables declaradas
        }
    }

    Expect(TokenType.In);
    Consume(); // Consume 'in'

    // Guarda las variables declaradas en el diccionario global
    foreach (var kvp in declaredVariables)
    {
        variables[kvp.Key] = kvp.Value;
    }

    Node statement = ParseStatement();
    return new LetInNode(variables, statement);
}
    private Node ParseTerm()
    {
        Node left = ParseFactor();

        while (Match(TokenType.Operator, "*") || Match(TokenType.Operator, "/") || Match(TokenType.Operator, "^"))
        {
            Token operatorToken = Consume();
            Node right = ParseFactor();
            left = new BinaryOperationNode(left, operatorToken, right);
        }

        return left;
    }
    private Node ParseParenthesizedExpression()
    {
        Expect(TokenType.Parenthesis, "(");
        Consume(); // Consume '('
        Node expression = ParseExpression();
        Expect(TokenType.Parenthesis, ")");
        Consume(); // Consume ')'
        return expression;
    }

    private Node ParseFactor()
{
    if (Match(TokenType.PI))
    {
        return new NumberNode(Consume());
    }
    if (Match(TokenType.Parenthesis, "("))
    {
        return ParseParenthesizedExpression();
    }
    if (Match(TokenType.Number))
    {
        return new NumberNode(Consume());
    }
    
    else if (Match(TokenType.String))
    {
        return new StringNode(Consume());
    }
    else if (Match(TokenType.If))
{
    Consume(); // Consume 'if'
    Node condition = ParseParenthesizedExpression() as BooleanNode;
    if (condition == null)
    {
        throw new Exception("Error de sintaxis: La condición del 'if' debe evaluar a un valor booleano.");
    }
    Node ifBranch = ParseStatement();

    if (Match(TokenType.Else))
    {
        Consume(); // Consume 'else'
        Node elseBranch = ParseStatement();
        return new IfElseNode(condition, ifBranch, elseBranch);
    }
    else
    {
        return new IfNode(condition, ifBranch);
    }
}
    else if (Match(TokenType.Parenthesis, "("))
    {
        Consume(); // Consume '('
        Node expression = ParseExpression();
        Expect(TokenType.Parenthesis, ")");
        return expression;
    }
    else if (Match(TokenType.Print))
    {
        Consume(); // Consume 'print'
        Expect(TokenType.Parenthesis, "(");
        Consume(); // Consume '('
        Node expression = ParseExpression();
        Expect(TokenType.Parenthesis, ")");
        // No necesitas consumir ')' nuevamente aquí, ya que lo hiciste arriba
        return new PrintNode(expression);
    }
    
    else if (Match(TokenType.UnaryOperator, "-"))
    {
        Consume(); // Consume el operador unario '-'
        Node factor = ParseFactor();
        return new UnaryOperationNode("-", factor);
    }
    else if (Match(TokenType.CosFunction))
    {
        Consume(); // Consume el token "cos"
        Node argument = ParseParenthesizedExpression(); 
        return new MathFunctionNode("cos", argument);
    }
    else if (Match(TokenType.SenFunction))
    {
        Consume(); // Consume el token "sin"
        Node argument = ParseParenthesizedExpression(); 
        return new MathFunctionNode("sen", argument);
    }
    else if (Match(TokenType.LogFunction))
    {
       Consume(); // Consume el token "log"
        Expect(TokenType.Parenthesis, "(");
        Consume(); // Consume '('

        // Parsea la base del logaritmo
        Node baseExpression = ParseExpression();

        Expect(TokenType.Coma, ",");
        Consume(); // Consume ','

        // Parsea el número al que se aplica el logaritmo
        Node numberExpression = ParseExpression();

        Expect(TokenType.Parenthesis, ")");
        Consume(); // Consume ')'

        return new LogFunctionNode(baseExpression, numberExpression);
    }
    else if (Match(TokenType.Identifier))
{
    Token FunctionName = Consume();
    string functionName=FunctionName.Value;

    if (Match(TokenType.Parenthesis, "("))
    {
        Consume(); // Consume '('
        List<Node> arguments = new List<Node>();

        while (!Match(TokenType.Parenthesis, ")"))
        {
            Node argument = ParseExpression();
            arguments.Add(argument);

            if (Match(TokenType.Coma))
            {
                Consume(); // Consume ','
            }
        }

        Expect(TokenType.Parenthesis, ")");
        Consume(); // Consume ')'

        if (userDefinedFunctions.ContainsKey(functionName)||functionName==FunctionName.Value)
        {
            // Encuentra la función definida por el usuario
            FunctionDeclarationNode functionDeclaration = userDefinedFunctions[functionName];

            // Verifica si el número de argumentos coincide con la declaración de la función
            if (functionDeclaration.Parameters.Count != arguments.Count)
            {
                throw new Exception($"!SEMANTIC ERROR :La función '{functionName}' recibe '{functionDeclaration.Parameters.Count}' argumentos, no'{arguments.Count} ");
            }

            // Crea un nuevo diccionario para las variables locales de la función
            Dictionary<string, Node> localVariables = new Dictionary<string, Node>();

            // Agrega los parámetros al diccionario local
            for (int i = 0; i < functionDeclaration.Parameters.Count; i++)
            {
                localVariables[functionDeclaration.Parameters[i]] = arguments[i];
            }

            // Agrega las variables globales al diccionario local
            foreach (var variable in variables)
            {
                localVariables[variable.Key] = variable.Value;
            }

            // Evalúa la función con las variables locales
            Node result = functionDeclaration.Body.EvaluateWithVariables(localVariables) as Node;

            // Agrega las variables locales nuevamente al diccionario global
            foreach (var kvp in localVariables)
            {
                variables[kvp.Key] = kvp.Value;
            }

            return result;
        }
        else
        {
            throw new Exception($"Función no definida: {functionName}");
        }
    }
    else
    {
        // Variable
        if (variables.ContainsKey(functionName))
        {
            return variables[functionName];
        }
        if (parameters.Contains(functionName))
        {
            return  new VariableNode(FunctionName);
        }
        else
        {
            throw new Exception($"Variable no definida: {functionName}");
        }
    }
}
    /*else if (Match(TokenType.Operator, ">"))
    {
        Consume(); // Consume el operador de comparación '>'
        Node right = ParseExpression(); // Parsea la expresión de la derecha
        return new BooleanNode(left, new Token(TokenType.Operator, ">"), right);
    }
    */
    else if (Match(TokenType.Let))
        {
        return ParseLetIn();
        }
    else
    {
        string currentTokenValue = tokens[currentTokenIndex].Value;
    throw new Exception($"!SYNTAX ERROR: Token inesperado '{currentTokenValue}' en la posición {currentTokenIndex}");
    }
}

    private bool Match(TokenType type, string value = null)
        {
            if (currentTokenIndex < tokens.Length)
            {
                Token currentToken = tokens[currentTokenIndex];
                if (currentToken.Type == type && (value == null || currentToken.Value == value))
                {
                    return true;
                }
            }
            return false;
        }

        private bool Expect(TokenType type, string value = null)
        {
            if (Match(type, value))
            {
                return true;
            }
            else
            {
                Token currentToken = tokens[currentTokenIndex];
                throw new Exception($"!SYNTAX ERROR: Se esperaba un token de tipo '{type}' con valor '{value ?? "ninguno"}', pero se encontró '{currentToken.Type}' con valor '{currentToken.Value}' en la posición {currentTokenIndex}");
            }
        }

        private Token Consume()
        {
            Token currentToken = tokens[currentTokenIndex];
            currentTokenIndex++;
            return currentToken;
        }
    }
}


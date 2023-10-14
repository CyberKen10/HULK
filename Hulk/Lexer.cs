
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
namespace Hulk
{
class Lexer
{
    private string input;         // La cadena de código fuente de entrada.
    public List<Token> tokens;   // Lista para almacenar los tokens encontrados.
    private int currentPosition;  // La posición actual en la cadena de entrada.

    public Lexer(string input)
    {
        this.input = input;           // Inicializa la cadena de código fuente.
        this.tokens = new List<Token>();  // Inicializa la lista de tokens.
        this.currentPosition = 0;     // Inicializa la posición actual en 0.
    }

    // Método principal para dividir la cadena en tokens.
    public Token[] Tokenize()
    {
        while (currentPosition < input.Length)
        {
            char currentChar = input[currentPosition];

            if (char.IsWhiteSpace(currentChar))
            {
                currentPosition++;
                continue;
            }
            else if ((currentChar == '-' && IsUnaryOperator()))
            {
                Token token=ScanUnaryOperator();
                tokens.Add(token);
            }
            else if (currentChar == 'c' && MatchKeyword("cos"))
            {
                Token token = new Token(TokenType.CosFunction, "cos");
                tokens.Add(token);
                currentPosition += 3; // Saltar "cos"
            }
            else if (currentChar == 's' && MatchKeyword("sen"))
            {
                Token token = new Token(TokenType.SenFunction, "sen");
                tokens.Add(token);
                currentPosition += 3; // Saltar "sin"
            }
            else if (currentChar == 'l' && MatchKeyword("log"))
            {
                Token token = new Token(TokenType.LogFunction, "log");
                tokens.Add(token);
                currentPosition += 3; // Saltar "log"
            }
            else if (char.IsDigit(currentChar))
            {
                Token token = ScanNumber();
                tokens.Add(token);
            }
            else if (char.IsLetter(currentChar))
            {
                Token token = ScanIdentifierOrKeyword();
                tokens.Add(token);
            }
            else if (currentChar == '=' && MatchArrow())
            {
                Token token = new Token(TokenType.Arrow, "=>");
                tokens.Add(token);
                currentPosition += 2; // Saltar "=>"
            }
            else if (IsOperator(currentChar))
            {
                string token= ScanOperator();
                tokens.Add(new Token(TokenType.Operator, token));
            }
            else if (currentChar == '(' || currentChar == ')')
            {
                tokens.Add(new Token(TokenType.Parenthesis, currentChar.ToString()));
                currentPosition++;
            }
            else if (currentChar == ';')
            {
                tokens.Add(new Token(TokenType.PuntoYComa, currentChar.ToString()));
                currentPosition++;
            }
            else if (currentChar == ',')
            {
                tokens.Add(new Token(TokenType.Coma,currentChar.ToString())); 
                currentPosition++;
            }
            else if (currentChar=='"')
            {
                Token token=ScanString();
                tokens.Add(token);
            }
            else
            {
                throw new Exception($"Carácter no válido: {currentChar}");
            }
        }
       if (input.TrimEnd().EndsWith(";"))
        {
            return tokens.ToArray();
        }
        else
        {
            throw new Exception("!SYNTAX ERROR: La entrada debe terminar con un punto y coma (;).");
        }
    }
    private bool IsUnaryOperator()
    {
        int nextPosition = currentPosition + 1;
        if (nextPosition < input.Length && char.IsDigit(input[nextPosition]))
        {
            // Verifica si el '-' es un operador unario
            return true;
        }
        return false;
    }
    // Método para escanear y reconocer un número.
    private Token ScanNumber()
{
    string number = "";
    while (currentPosition < input.Length && (char.IsDigit(input[currentPosition]) || input[currentPosition] == '.'))
    {
        number += input[currentPosition];
        currentPosition++;
    }
    if (currentPosition < input.Length && (char.IsLetter(input[currentPosition]) || char.IsDigit(input[currentPosition])))
{
    int errorPosition = currentPosition;
    while (errorPosition < input.Length && (char.IsLetter(input[errorPosition]) || char.IsDigit(input[errorPosition])))
    {
        errorPosition++;
    }
    
    string invalidToken = input.Substring(currentPosition-1, errorPosition - currentPosition+1);
    throw new Exception($"!LEXICAL ERROR: {invalidToken} no es un token válido");
}

    if (number.Contains("."))
    {
        if (double.TryParse(number, out double decimalValue))
        {
            return new Token(TokenType.Number, decimalValue.ToString()); // Devolvemos el valor decimal como un número
        }
        else
        {
            throw new Exception($"Número no válido: {number}");
        }
    }
    else
    {
        if (int.TryParse(number, out int intValue))
        {
            return new Token(TokenType.Number, intValue.ToString()); // Devolvemos el valor entero como un número
        }
        else
        {
            throw new Exception($"Número no válido: {number}");
        }
    }
}

    // Método para escanear y reconocer un identificador.
    private Token ScanIdentifierOrKeyword()
    {
        string identifier = "";
        while (currentPosition < input.Length && (char.IsLetterOrDigit(input[currentPosition]) || input[currentPosition] == '_'))
        {
            identifier += input[currentPosition];
            currentPosition++;
        }
        if (!IsIdentifierValid(identifier))
    {
        // Reportar un "LEXICAL ERROR"
        throw new Exception($"LEXICAL ERROR: Identificador no válido: {identifier}");
    }
        
         if (identifier == "PI")
        {
            return new Token(TokenType.PI, Math.PI.ToString());
        }
        // Verifica si el identificador es una palabra clave.
        if (identifier == "if")
        {
            return new Token(TokenType.If, identifier);
        }
        else if (identifier == "else")
        {
            return new Token(TokenType.Else, identifier);
        }
        else if (identifier == "let")
        {
            return new Token(TokenType.Let, identifier);
        }
        else if (identifier == "in")
        {
            return new Token(TokenType.In, identifier);
        }
        else if (identifier=="print")
        {
            return new Token(TokenType.Print,identifier);
        }
        else if (identifier == "function") // Nueva palabra clave 'function'
        {
            return new Token(TokenType.FunctionDeclaration, identifier);
        }
        return new Token(TokenType.Identifier, identifier);
    }
    // Método para verificar si un carácter es un operador válido.
    private bool IsOperator(char c)
    {
        char[] validOperators = { '+', '-', '*', '/', '^', '=', '<', '>', '!','@','%'};
        return Array.IndexOf(validOperators, c) != -1;
    }
    private bool MatchArrow()
    {
        int nextPosition = currentPosition + 1;
        if (nextPosition < input.Length && input[nextPosition] == '>')
        {
            return true;
        }
        return false;
    }
    private bool MatchKeyword(string keyword)
    {
    int keywordLength = keyword.Length;
    if (currentPosition + keywordLength <= input.Length)
    {
        string potentialKeyword = input.Substring(currentPosition, keywordLength);
        return potentialKeyword == keyword;
    }
    return false;
    }
    private Token ScanUnaryOperator()
    {
        int startPosition = currentPosition;
        while (currentPosition < input.Length && char.IsWhiteSpace(input[currentPosition]))
        {
            currentPosition++; // Saltar espacios en blanco
        }

        if (currentPosition < input.Length && input[currentPosition] == '-')
        {
            currentPosition++;
            return new Token(TokenType.UnaryOperator, "-");
        }

        currentPosition = startPosition;
        return null; // No se encontró un operador unario
    }
    private Token ScanString()
{
    currentPosition++; // Salta la comilla doble inicial
    string value = "";

    while (currentPosition < input.Length)
    {
        char currentChar = input[currentPosition];
        if (currentChar == '"')
        {
            currentPosition++; // Salta la comilla doble final
            return new Token(TokenType.String, value);
        }

        value += currentChar;
        currentPosition++;
    }

    throw new Exception("!SYNTAX ERROR: Falta una comilla doble de cierre.");
}
private bool IsIdentifierValid(string identifier)
{
    // Verificar si el identificador contiene solo letras, dígitos o guiones bajos
    return System.Text.RegularExpressions.Regex.IsMatch(identifier, "^[a-zA-Z0-9_]+$");
}
private string ScanOperator()
{
    string operatorValue = "";

    while (currentPosition < input.Length && IsOperator(input[currentPosition]))
    {
        operatorValue += input[currentPosition];
        currentPosition++;
    }

    return operatorValue;
}

}
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
namespace Hulk
{
class Compiler
{
   static void Main(string[] args)
    {  
    DisplayHulkCompilerBanner(); // Mostrar el diseño estético al inicio
    Dictionary<string, FunctionDeclarationNode> userDefinedFunctions = new Dictionary<string, FunctionDeclarationNode>();
    while (true)
    {
        
        Console.ForegroundColor = ConsoleColor.DarkMagenta; // Establece el color del texto en morado
        Console.Write(">> "); // Imprime ">> " en la consola
        string input =Console.ReadLine(); //
        if (input?.ToLower() == "exit")
        {
            break; // Salir del bucle si el usuario ingresa 'exit'
        }

        try
        {
            Lexer lexer = new Lexer(input);
            Parser parser = new Parser(lexer, userDefinedFunctions);
            Node expression = parser.Parse();
            object result = expression.Evaluate();

            Console.ForegroundColor = ConsoleColor.White; // Establece el color del texto en amarillo

            if (result is int)
            {
                Console.WriteLine(">" + (int)result);
            }
            else if (expression is FunctionDeclarationNode)
            {
                FunctionDeclarationNode? functionNode = expression as FunctionDeclarationNode;
                userDefinedFunctions[functionNode.Name] = functionNode;
            }
            else if (result is double)
            {
                Console.WriteLine("> " + (double)result);
            }
            else if (result is string)
            {
                Console.WriteLine("> " + (string)result);
            }
            else
            {
                Console.WriteLine("Resultado desconocido: " + result.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Establece el color del texto en rojo
            Console.WriteLine(ex.Message);
            Console.ResetColor(); // Restablece el color del texto a su valor predeterminado
        }
    }
}

static void DisplayHulkCompilerBanner()
{
    Console.ForegroundColor = ConsoleColor.DarkMagenta; // Establece el color del texto en morado
    Console.BackgroundColor = ConsoleColor.Black;    // Color de fondo

    Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║                                                                ║");
    Console.WriteLine("║                  >>Welcome to Hulk Compiler<<                  ║");
    Console.WriteLine("║                                                                ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");

    Console.ResetColor(); // Restablece los colores a sus valores predeterminados
}
}
}





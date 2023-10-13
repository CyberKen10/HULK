using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Hulk
{
    enum TokenType
{
    Number,
    Identifier,
    Operator,
    Print,
    Parenthesis,
    PuntoYComa,
    Power,
    Assign,
    Let,
    In,
    If,
    Else,
    UnaryOperator,
    String,
    Coma,
    CosFunction,
    SenFunction,
    LogFunction,
    Arrow,
    FunctionDeclaration,
    PI

    

}


class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Lenght;

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
        Lenght= value.Length;
    }

    public override string ToString()
    {
        return $"({Type}, '{Value}')";
    }
}
}
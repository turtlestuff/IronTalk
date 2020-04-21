using System.Collections.Generic;
using Microsoft.Scripting;
using TurtleSpeak.Compiler.Syntax;

namespace TurtleSpeak.Compiler.Expressions
{
    public class Parser
    {
        readonly Lexer lexer;
        Token current;
        Token? peek;

        public Parser(Lexer l)
        {
            lexer = l;
            current = lexer.Lex();
        }

        Token NextToken()
        {
            var c = current;
            current = peek ?? lexer.Lex();
            peek = null;
            return current;
        }

        Token Peek() => peek ??= lexer.Lex();

        static SourceSpan From(Token left, SourceLocation right) => new SourceSpan(left.Location.Start, right);
        static SourceSpan From(Token left, Token right) => new SourceSpan(left.Location.Start, right.Location.End);
    }
}
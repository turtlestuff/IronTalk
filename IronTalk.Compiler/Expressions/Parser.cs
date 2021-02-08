using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Scripting;
using IronTalk.Compiler.Syntax;

namespace IronTalk.Compiler.Expressions
{
    public class Parser
    {
        internal readonly Lexer lexer;
        Token current;
        Token? peek;

        public Parser(Lexer l)
        {
            lexer = l;
            peek = lexer.Lex();
            current = peek;
        }

        Token NextToken()
        {
            current = peek ?? lexer.Lex();
            peek = null;
            return current;
        }

        public ImmutableArray<Node> ParseEverything()
        {
            var builder = ImmutableArray.CreateBuilder<Node>();

            while (true)
            {
                var t = NextToken();
                if (t is SyntaxToken s && s.Kind == SyntaxTokenKind.Eof) break;
                builder.Add(ParseNode(t));
            }

            return builder.ToImmutable();
        }

        Node ParseNode(Token t)
        {
            return t switch
            {
                KeywordToken { Kind: KeywordTokenKind.Using } k => ParseUsingStatement(k),
                IdentifierToken i => ParseClassDeclaration(i),
                _ => throw new NotImplementedException($"{t.Location}")
                //TODO: make into diagnostic
            };
        }

        ClassDeclaration ParseClassDeclaration(IdentifierToken identifier)
        {
            return null!;   
        }

        UsingStatement ParseUsingStatement(KeywordToken token)
        {
            var names = new List<IdentifierToken>();
            SourceSpan dotPosition;
            while (true)
            {
                var t = NextToken();
                if (t is IdentifierToken name)
                {
                    names.Add(name);
                }
                else if (t is SyntaxToken { Kind: SyntaxTokenKind.Dot })
                {
                    dotPosition = t.Location;
                    break;
                }
                else
                {
                    throw new Exception($"Invalid Using Directive @ {t.Location}");
                }
                //TODO: make into diagnostic
            }
            return new UsingStatement(names.ToArray(), new SourceSpan(token.Location.Start, dotPosition.End));
        }


        Token Peek() => peek ??= lexer.Lex();

        static SourceSpan From(Token left, SourceLocation right) => new SourceSpan(left.Location.Start, right);
        static SourceSpan From(Token left, Token right) => new SourceSpan(left.Location.Start, right.Location.End);
    }
}
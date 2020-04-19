using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace TurtleSpeak.Compiler.Syntax
{
    public class Lexer
    {
        const char Eof = unchecked((char) -1); //eof charcter

        readonly char[] SpecialCharacters = {'+', '/', '\\', '.', '~', '<', '>', '=', '@', '%', '!', '&', '?', '!', '-'};

        //public CompilerContext Context { get; }
        //SourceCodeReader sourceCode;
        TokenizerBuffer reader;

        public Lexer(TextReader r)
        {
            //Context = context;
            //sourceCode = context.SourceUnit.GetReader();
            reader = new TokenizerBuffer(r, new SourceLocation(0, 1, 1),
                10, true);
        }

        public IEnumerable<Token> Lex()
        {
            doItAgain:
            SkipWhitespace();
            var ch = (char) reader.Peek();

            if (ch != Eof) reader.DiscardToken();
            switch (ch)
            {
                case Eof:
                    yield return new SyntaxToken(SyntaxTokenKind.Eof, reader.TokenSpan);
                    yield break;
                case ':':
                    reader.Read();
                    if (reader.Read('='))
                    {
                        reader.MarkSingleLineTokenEnd();
                        yield return new SyntaxToken(SyntaxTokenKind.ColonEquals, reader.TokenSpan);
                        break;
                    }

                    reader.MarkSingleLineTokenEnd();
                    yield return new SyntaxToken(SyntaxTokenKind.Colon, reader.TokenSpan);
                    break;
                case '.':
                    yield return LexSingleOperator(SyntaxTokenKind.Dot);
                    break;
                case '#':
                    reader.Read();
                    if (reader.Read('('))
                    {
                        reader.MarkSingleLineTokenEnd();
                        yield return new SyntaxToken(SyntaxTokenKind.HashOpenParenthesis, reader.TokenSpan); //array
                        break;
                    }
                    else if (reader.Read('['))
                    {
                        reader.MarkSingleLineTokenEnd();
                        yield return new SyntaxToken(SyntaxTokenKind.HashOpenBracket, reader.TokenSpan); //byte array
                        break;
                    }
                    else if (reader.Peek() == '"')
                    {
                        var s = ReadStringLiteral();
                        yield return new SymbolToken(s[2..^1], reader.TokenSpan);
                        break;
                    }
                    else if (char.IsLetter((char) reader.Peek()))
                    {
                        var s = ReadIdentifier();
                        yield return new SymbolToken(s[1..], reader.TokenSpan);
                        break;
                    }
                    else if (IsSpecialCharacter((char) reader.Peek()))
                    {
                        var s = ReadBinarySelector();
                        yield return new SymbolToken(s[1..], reader.TokenSpan);
                        break;
                    }
                    
                    break;
                case '^':
                    yield return LexSingleOperator(SyntaxTokenKind.Hat);
                    break;
                case '[':
                    yield return LexSingleOperator(SyntaxTokenKind.OpenBracket);
                    break;
                case ']':
                    yield return LexSingleOperator(SyntaxTokenKind.CloseBracket);
                    break;
                case '(':
                    yield return LexSingleOperator(SyntaxTokenKind.OpenParenthesis);
                    break;
                case ')':
                    yield return LexSingleOperator(SyntaxTokenKind.CloseParenthesis);
                    break;
                case '|':
                    yield return LexSingleOperator(SyntaxTokenKind.Pipe);
                    break;
                case ';':
                    yield return LexSingleOperator(SyntaxTokenKind.Semicolon);
                    break;
                default:
                    reader.Read();
                    reader.MarkSingleLineTokenEnd();
                    yield return new SyntaxToken(SyntaxTokenKind.Invalid, reader.TokenSpan);
                    break;
            }

            goto doItAgain;
        }

        static readonly char[] WhitespaceChars = {' ', '\r', '\n', '\t'};

        void SkipWhitespace()
        {
            for (var ch = reader.Peek(); Array.IndexOf(WhitespaceChars, (char) ch) != -1; ch = reader.Peek())
                reader.Read();

            reader.MarkMultiLineTokenEnd();
        }

        SyntaxToken LexSingleOperator(SyntaxTokenKind stk)
        {
            reader.Read();
            reader.MarkSingleLineTokenEnd();
            return new SyntaxToken(stk, reader.TokenSpan);
        }

        string ReadStringLiteral()
        {
            reader.Read();
            while (true)
            {
                var c = (char) reader.Read();
                if (c != '"') continue;
                reader.MarkSingleLineTokenEnd();
                break;
            }

            return reader.GetTokenString();
        }

        string ReadIdentifier()
        {
            reader.Read();
            while (true)
            {
                var c = (char) reader.Read();
                if(char.IsLetterOrDigit(c)) continue;
                reader.MarkSingleLineTokenEnd();
                break;
            }

            return reader.GetTokenString();
        }

        string ReadBinarySelector()
        {
            reader.Read();
            if (!IsSpecialCharacter((char) reader.Peek()))
            {
                reader.MarkSingleLineTokenEnd();
                return reader.GetTokenString();
            }

            reader.Read();
            reader.MarkSingleLineTokenEnd();
            return reader.GetTokenString();
        }

        bool IsSpecialCharacter(char c) => Array.IndexOf(SpecialCharacters, c) != -1;
    }
}
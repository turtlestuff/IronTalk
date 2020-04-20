using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Numerics;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace TurtleSpeak.Compiler.Syntax
{
    public class Lexer
    {
        const char Eof = unchecked((char) -1); //eof charcter

        readonly char[] SpecialCharacters =
            {'+', '/', '\\', '.', '~', '<', '>', '=', '@', '%', '!', '&', '?', '!', '-'};

        //public CompilerContext Context { get; }
        //SourceCodeReader sourceCode;
        readonly TokenizerBuffer reader;

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

            reader.DiscardToken();
            switch (ch)
            {
                case Eof:
                    yield return new SyntaxToken(SyntaxTokenKind.Eof, reader.TokenSpan);
                    yield break;
                case ':':
                    reader.Read();
                    var kind = reader.Read('=') ? SyntaxTokenKind.ColonEquals : SyntaxTokenKind.Colon;
                    reader.MarkSingleLineTokenEnd();
                    yield return new SyntaxToken(kind, reader.TokenSpan);
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
                case '"':
                    yield return new StringToken(ReadStringLiteral()[1..^1], reader.TokenSpan);
                    break;
                default:

                    if (char.IsDigit(ch) || ch == '-' || ch == '+')
                    {
                        yield return ReadNumber();
                        break;
                    }

                    if (char.IsLetter(ch))
                    {
                        yield return ReadIdOrKeywordToken();
                        break;
                    }

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
            while (char.IsLetterOrDigit((char) reader.Peek()))
                reader.Read();

            reader.MarkSingleLineTokenEnd();
            return reader.GetTokenString();
        }

        IdOrKeywordToken ReadIdOrKeywordToken()
        {
            reader.Read();
            while (char.IsLetterOrDigit((char) reader.Peek()))
                reader.Read();
            reader.MarkSingleLineTokenEnd();

            return Enum.TryParse<KeywordTokenKind>(reader.GetTokenString(), true, out var b)
                ? new KeywordToken(b, reader.TokenSpan)
                : new IdOrKeywordToken(reader.GetTokenString(), reader.TokenSpan);
        }

        string ReadBinarySelector()
        {
            reader.Read();
            if (IsSpecialCharacter((char) reader.Peek()))
                reader.Read();

            reader.MarkSingleLineTokenEnd();
            return reader.GetTokenString();
        }

        NumberToken ReadNumber()
        {
            reader.Read();
            while (char.IsDigit((char) reader.Peek()) || (char) reader.Peek() == 'e' || (char) reader.Peek() == '-' ||
                   (char) reader.Peek() == '.')
                reader.Read();
            reader.MarkSingleLineTokenEnd();
            var numberString = reader.GetTokenString();
            if (numberString.Contains('.') || numberString.Contains('e'))
            {
                if (double.TryParse(numberString, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
                    return new NumberToken(d, reader.TokenSpan);
            }
            else if (int.TryParse(numberString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            {
                return new NumberToken(i, reader.TokenSpan);
            }
            else if (long.TryParse(numberString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var l))
            {
                return new NumberToken(l, reader.TokenSpan);
            }
            else if (BigInteger.TryParse(numberString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var b))
            {
                return new NumberToken(b, reader.TokenSpan);
            }

            throw new FormatException($"bad number format at {reader.TokenSpan}");
        }

        bool IsSpecialCharacter(char c) => Array.IndexOf(SpecialCharacters, c) != -1;
    }
}
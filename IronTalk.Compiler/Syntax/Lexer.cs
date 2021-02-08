using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace IronTalk.Compiler.Syntax
{
    public class Lexer
    {
        const char Eof = unchecked((char) -1); //eof charcter

        readonly char[] specialCharacters =
            {'+', '/', '\\', '.', '~', '<', '>', '=', '@', '%', '!', '&', '?', '!', '-'};

        //public CompilerContext Context { get; }
        //SourceCodeReader sourceCode;
        readonly TokenizerBuffer reader;

        readonly string[] keywordNames =
            Enum.GetNames(typeof(KeywordTokenKind)).Select(s => s.ToLowerInvariant()).ToArray();

        public Lexer(TextReader r)
        {
            //Context = context;
            //sourceCode = context.SourceUnit.GetReader();
            reader = new TokenizerBuffer(r, new SourceLocation(0, 1, 1),
                10, true);
        }

        public Token Lex()
        {
            doItAgain:
            SkipWhitespace();
            var ch = (char) reader.Peek();

            reader.DiscardToken();
            switch (ch)
            {
                case Eof:
                    return new SyntaxToken(SyntaxTokenKind.Eof, reader.TokenSpan);
                case '"':
                    SkipComment();
                    break;
                case ':':
                    reader.Read();
                    var kind = reader.Read('=') ? SyntaxTokenKind.ColonEquals : SyntaxTokenKind.Colon;
                    reader.MarkSingleLineTokenEnd();
                    return new SyntaxToken(kind, reader.TokenSpan);
                case '.':
                    return LexSingleOperator(SyntaxTokenKind.Dot);
                case '#':
                    reader.Read();
                    if (reader.Read('('))
                    {
                        reader.MarkSingleLineTokenEnd();
                        return new SyntaxToken(SyntaxTokenKind.HashOpenParenthesis, reader.TokenSpan); //array
                    }

                    if (reader.Read('['))
                    {
                        reader.MarkSingleLineTokenEnd();
                        return new SyntaxToken(SyntaxTokenKind.HashOpenBracket, reader.TokenSpan); //byte array
                    }

                    if (reader.Peek() == '\'')
                    {
                        var s = ReadStringLiteral();
                        return new SymbolToken(s[2..^1], reader.TokenSpan);
                    }

                    if (char.IsLetter((char) reader.Peek()))
                    {
                        var s = ReadIdentifier();
                        return new SymbolToken(s[1..], reader.TokenSpan);
                    }

                    if (IsSpecialCharacter((char) reader.Peek()))
                    {
                        var s = ReadBinarySelector();
                        return new SymbolToken(s[1..], reader.TokenSpan);
                    }

                    reader.MarkSingleLineTokenEnd();
                    return new SyntaxToken(SyntaxTokenKind.Invalid, reader.TokenSpan);
                case '^':
                    return LexSingleOperator(SyntaxTokenKind.Hat);
                case '[':
                    return LexSingleOperator(SyntaxTokenKind.OpenBracket);
                case ']':
                    return LexSingleOperator(SyntaxTokenKind.CloseBracket);
                case '(':
                    return LexSingleOperator(SyntaxTokenKind.OpenParenthesis);
                case ')':
                    return LexSingleOperator(SyntaxTokenKind.CloseParenthesis);
                case '|':
                    return LexSingleOperator(SyntaxTokenKind.Pipe);
                case ';':
                    return LexSingleOperator(SyntaxTokenKind.Semicolon);
                case '\'':
                    return new StringToken(ReadStringLiteral()[1..^1], reader.TokenSpan);
                case '$':
                    reader.Read();
                    reader.MarkTokenEnd(reader.Read() == '\n');
                    //This seems weird, but this is standard Smalltalk behaviour (tested in squeak)
                    //Doing $ and putting a newline right after it will return a newline character.
                    //Also, something like '$ ' will indeed return a space.
                    return reader.Peek() != Eof
                        ? (Token) new CharacterToken(reader.GetTokenString()[^1], reader.TokenSpan)
                        : new SyntaxToken(SyntaxTokenKind.Invalid, reader.TokenSpan);
                default:

                    if (IsSpecialCharacter(ch))
                    {
                        return new IdentifierToken(ReadBinarySelector(), reader.TokenSpan);
                    }

                    if (char.IsDigit(ch) || ch == '-' || ch == '+')
                    {
                        return ReadNumber();
                    }

                    if (char.IsLetter(ch))
                    {
                        return ReadIdOrKeywordToken();
                    }

                    reader.Read();
                    reader.MarkSingleLineTokenEnd();
                    return new SyntaxToken(SyntaxTokenKind.Invalid, reader.TokenSpan);
            }

            goto doItAgain;
        }

        void SkipComment()
        {
            reader.Read();
            while (true)
            {
                var c = (char)reader.Read();
                if (c != '"') continue;
                reader.MarkMultiLineTokenEnd();
                break;
            }
        }

        void SkipWhitespace()
        {
            while (true)
            {
                if (!char.IsWhiteSpace((char)reader.Peek()))
                    break;
                reader.Read();
            }

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
                if (c != '\'') continue;
                reader.MarkMultiLineTokenEnd();
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

        Token ReadIdOrKeywordToken()
        {
            reader.Read();
            while (char.IsLetterOrDigit((char) reader.Peek()))
                reader.Read();
            reader.MarkSingleLineTokenEnd();

            var s = reader.GetTokenString();
            if (keywordNames.Contains(s))
                return new KeywordToken(Enum.Parse<KeywordTokenKind>(s, true), reader.TokenSpan);
            else
                return new IdentifierToken(s, reader.TokenSpan);

            
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

        bool IsSpecialCharacter(char c) => Array.IndexOf(specialCharacters, c) != -1;
    }
}
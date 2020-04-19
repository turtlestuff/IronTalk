using System;
using System.IO;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace TinySpeak.Compiler.Syntax
{
    public class Lexer
    {
        const char Eof = unchecked((char) -1); //eof charcter
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

        public Token Lex()
        {
            SkipWhitespace();
            var ch = (char) reader.Peek();

            if (ch != Eof) reader.DiscardToken();

            switch (ch)
            {
                case Eof:
                    return new SyntaxToken(SyntaxTokenKind.Eof, reader.TokenSpan);
                case '[':
                    reader.Read();
                    reader.MarkSingleLineTokenEnd();
                    return new SyntaxToken(SyntaxTokenKind.OpenBracket, reader.TokenSpan);
                default:
                    throw new Exception("IS POOP`t!");
            }
        }

        static readonly char[] WhitespaceChars = {' ', '\r', '\n', '\t'};

        void SkipWhitespace()
        {
            for (var ch = reader.Peek(); Array.IndexOf(WhitespaceChars, (char) ch) != -1; ch = reader.Peek())
                reader.Read();

            reader.MarkMultiLineTokenEnd();
        }
    }
}
using System;
using System.IO;
using Microsoft.Scripting.Runtime;
using TinySpeak.Compiler.Syntax;

namespace TinySpeak
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                throw new NotImplementedException("we dont have fancy yet");
            }

            while (true)
            {
                var sorc = Console.ReadLine();
                var lexr = new Lexer(new StringReader(sorc ?? ""));
                Token token;
                while (!((token = lexr.Lex()) is SyntaxToken {Kind: SyntaxTokenKind.Eof}))
                {
                    Console.WriteLine(token);
                }
            }
        }
    }
}
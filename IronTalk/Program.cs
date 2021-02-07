using System;
using System.IO;
using IronTalk.Compiler.Expressions;
using IronTalk.Compiler.Syntax;

namespace IronTalk
{
    static class Program
    {
        static void Main(string[] args)
        { 

            Console.WriteLine("Irontalk Test Parser");
            while (true)
            {
                Console.Write(">=>");
                var source = Console.ReadLine();
                var lexer = new Lexer(new StringReader(source ?? ""));
                while (lexer.Lex() is var l ^ l is SyntaxToken { Kind: SyntaxTokenKind.Eof })
                    Console.WriteLine(l);
                Console.WriteLine("\n~~Parse~~");
                lexer = new Lexer(new StringReader(source ?? ""));
                var parser = new Parser(lexer);
                foreach (var n in parser.ParseEverything())
                    Console.WriteLine(n);
            }
        }
    }
}
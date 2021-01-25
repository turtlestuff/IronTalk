using System;
using System.IO;
using TurtleSpeak.Compiler.Expressions;
using TurtleSpeak.Compiler.Syntax;

namespace TurtleSpeak
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                throw new NotImplementedException("we dont have fancy yet");
            }

            Console.WriteLine(@"
It's the very very very good
TurtleSpeak Parser!!!!!!!!");
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
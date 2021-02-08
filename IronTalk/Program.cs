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
                try
                {
                    Console.Write(">=>");
                    var sourceLines = new System.Collections.Generic.List<string>();
                    while (true)
                    {
                        var src = Console.ReadLine();
                        if (src == "")
                            break;
                        sourceLines.Add(src);
                    }
                    var source = String.Join('\n', sourceLines);
                    var lexer = new Lexer(new StringReader(source ?? ""));
                    while (lexer.Lex() is var l ^ l is SyntaxToken { Kind: SyntaxTokenKind.Eof })
                        Console.WriteLine(l);
                    Console.WriteLine("\n~~Parse~~");
                    lexer = new Lexer(new StringReader(source ?? ""));

                    var parser = new Parser(lexer);
                    foreach (var n in parser.ParseEverything())
                        Console.WriteLine(n);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    Console.Error.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
using System;
using System.IO;
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

            while (true)
            {
                var source = Console.ReadLine();
                var lexer = new Lexer(new StringReader(source ?? ""));
                foreach(var l in lexer.Lex())
                {
                    Console.WriteLine(l);
                }
            }
        }
    }
}
using System;
using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    public class SymbolToken : LiteralToken
    {
        public SymbolToken(string val, SourceSpan location) : base(string.Intern(val), location)
        {
        }
    }
}
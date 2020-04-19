using System;
using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    class SymbolToken : LiteralToken
    {
        public SymbolToken(string val, SourceSpan location) : base(string.Intern(val), location)
        {
        }
    }
}
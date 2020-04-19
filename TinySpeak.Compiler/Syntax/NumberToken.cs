using System;
using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    class NumberToken : LiteralToken
    {
        public NumberToken(double val, SourceSpan location) : base(val, location)
        {
        }
        
        public NumberToken(int val, SourceSpan location) : base(val, location)
        {
        }
    }
}
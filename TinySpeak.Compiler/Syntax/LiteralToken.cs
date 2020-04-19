using System;
using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    abstract class LiteralToken : Token
    {
        public object Value { get; }

        public LiteralToken(object val, SourceSpan location) : base(location) => Value = val;
    }
}
using Microsoft.Scripting;

namespace IronTalk.Compiler.Syntax
{
    public abstract class LiteralToken : Token
    {
        public object Value { get; }

        public LiteralToken(object val, SourceSpan location) : base(location) => Value = val;
    }
}
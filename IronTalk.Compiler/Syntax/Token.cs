using Microsoft.Scripting;

namespace IronTalk.Compiler.Syntax
{
    public abstract class Token
    {
        public SourceSpan Location { get; }
        protected Token(SourceSpan location) => Location = location;
    }
}
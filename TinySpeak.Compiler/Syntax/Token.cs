using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    public abstract class Token
    {
        public SourceSpan Location { get; }
        protected Token(SourceSpan location) => Location = location;
        
    }
}
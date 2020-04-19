using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    abstract class Token
    {
        public SourceSpan Location { get; }
        protected Token(SourceSpan location) => Location = location;
        
    }
}
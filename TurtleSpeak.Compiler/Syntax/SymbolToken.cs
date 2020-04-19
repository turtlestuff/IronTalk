using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
{
    public class SymbolToken : LiteralToken
    {
        public SymbolToken(string val, SourceSpan location) : base(string.Intern(val), location)
        {
        }

        public override string ToString() => $"SymbolToken {Value}@{Location}";
        
    }
}
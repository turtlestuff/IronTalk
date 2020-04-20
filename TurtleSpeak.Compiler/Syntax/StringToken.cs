using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
{
    public class StringToken : LiteralToken
    {
        public StringToken(string val, SourceSpan location) : base(val, location)
        {
        }
        
        public override string ToString() => $"StringToken \"{Value}\"@{Location}";
    }
}
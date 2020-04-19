using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
{
    public class NumberToken : LiteralToken
    {
        public NumberToken(double val, SourceSpan location) : base(val, location)
        {
        }
        
        public NumberToken(int val, SourceSpan location) : base(val, location)
        {
        }
    }
}
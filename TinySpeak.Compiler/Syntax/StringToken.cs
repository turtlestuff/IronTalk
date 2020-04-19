using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    class StringToken : LiteralToken
    {
        public StringToken(string val, SourceSpan location) : base(val, location)
        {
        }
    }
}
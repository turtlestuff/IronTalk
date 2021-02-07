using Microsoft.Scripting;

namespace IronTalk.Compiler.Syntax
{
    public class SymbolToken : LiteralToken
    {
        public SymbolToken(string val, SourceSpan location) : base(string.Intern(val), location)
        {
        }

        public override string ToString() =>
            $"SymbolToken #{(((string) Value).Contains(' ') ? "\"" : "")}{Value}{(((string) Value).Contains(' ') ? "\"" : "")}@{Location}";
    }
}
using Microsoft.Scripting;

namespace IronTalk.Compiler.Syntax
{
    public class CharacterToken : LiteralToken
    {
        public CharacterToken(char val, SourceSpan location) : base(val, location)
        {
        }

        public override string ToString() => $"CharacterToken ${Value}@{Location}";
    }
}
using Microsoft.Scripting;

namespace IronTalk.Compiler.Syntax
{
    public class IdentifierToken : Token
    {
        public string Name { get; }

        public IdentifierToken(string name, SourceSpan location) : base(location)
        {
            Name = name;
        }

        public override string ToString() => $"IdentifierToken {Name}@{Location}";
    }
}
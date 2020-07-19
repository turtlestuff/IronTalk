using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
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
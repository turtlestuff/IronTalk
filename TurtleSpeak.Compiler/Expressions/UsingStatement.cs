using Microsoft.Scripting;
using System.Linq;
using TurtleSpeak.Compiler.Syntax;

namespace TurtleSpeak.Compiler.Expressions
{
    public class UsingStatement : Node
    {
        public IdentifierToken[] Namespaces { get; }

        public UsingStatement(IdentifierToken[] namespaces, SourceSpan location) : base(location)
        {
            Namespaces = namespaces;
        }

        public override string ToString() => $"Using {string.Join('.', Namespaces.Select(i => i.Name))}";
    }                                                           
}
    
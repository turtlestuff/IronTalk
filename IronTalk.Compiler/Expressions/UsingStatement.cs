using Microsoft.Scripting;
using System.Linq;
using IronTalk.Compiler.Syntax;

namespace IronTalk.Compiler.Expressions
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
    
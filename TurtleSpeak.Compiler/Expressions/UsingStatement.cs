using Microsoft.Scripting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Xml.Serialization;
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

        public override string ToString() => $"Using {string.Join<IdentifierToken>('.', Namespaces)}";
    }                                                           //^^^Thanks, overloads :munamused:
}
    
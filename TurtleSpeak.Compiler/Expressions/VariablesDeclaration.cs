using Microsoft.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using TurtleSpeak.Compiler.Syntax;

namespace TurtleSpeak.Compiler.Expressions
{
    public class VariablesDeclaration : Node
    {
        public IdentifierToken[] Variables { get; }

        public VariablesDeclaration(IdentifierToken[] vars, SourceSpan location) : base(location)
        {
            Variables = vars;   
        }
    }
}

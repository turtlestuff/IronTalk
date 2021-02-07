using Microsoft.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using IronTalk.Compiler.Syntax;

namespace IronTalk.Compiler.Expressions
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

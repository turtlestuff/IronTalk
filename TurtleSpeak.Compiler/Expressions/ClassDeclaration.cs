using Microsoft.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using TurtleSpeak.Compiler.Syntax;

namespace TurtleSpeak.Compiler.Expressions
{
    public class ClassDeclaration : Node
    {
        public ClassDeclaration(SourceSpan location) : base(location) { }

        public IdentifierToken Name { get; }
        public IdentifierToken Inherits { get; }

        public object ClassFields { get; }
        public object ClassMethods { get; }
        public object InstFields { get; }
        public object InstMethods { get; }
    }
}

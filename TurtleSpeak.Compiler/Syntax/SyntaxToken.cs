using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
{
    public class SyntaxToken : Token
    {
        public SyntaxTokenKind Kind { get; }

        public SyntaxToken(SyntaxTokenKind kind, SourceSpan location) : base(location) => Kind = kind;

        public override string ToString()
        {
            return $"{Kind}@{Location}";
        }
    }
}
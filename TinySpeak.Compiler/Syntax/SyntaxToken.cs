using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    public class SyntaxToken : Token
    {
        public SyntaxTokenKind Kind { get; }

        public SyntaxToken(SyntaxTokenKind kind, SourceSpan location) : base(location) => Kind = kind;
    }
}
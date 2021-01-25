using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
{
    public class SyntaxToken : Token
    {
        public SyntaxTokenKind Kind { get; }

        public SyntaxToken(SyntaxTokenKind kind, SourceSpan location) : base(location) => Kind = kind;

        public override string ToString() => $"{Kind}@{Location}";
    }

    public enum SyntaxTokenKind
    {
        Eof = 0,
        Colon,
        ColonEquals,
        Dot,
        HashOpenBracket,
        HashOpenParenthesis,
        Hat,
        OpenBracket,
        CloseBracket,
        OpenParenthesis,
        CloseParenthesis,
        Pipe,
        Semicolon,
        Invalid
    }
}
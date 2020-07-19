using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
{
    public class KeywordToken : Token
    {
        public KeywordTokenKind Kind { get; }

        public KeywordToken(KeywordTokenKind kind, SourceSpan location) : base(location)
        {
            Kind = kind;
        }

        public override string ToString() => $"KeywordToken {Kind}@{Location}";
    }

    public enum KeywordTokenKind
    {
        True,
        False,
        Self,
        Super,
        Nil,
        Class,
        Inst,
        Using
    }
}
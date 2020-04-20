using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
{
    public class KeywordToken : IdOrKeywordToken
    {
        public KeywordToken(KeywordTokenKind name, SourceSpan location) : base(name.ToString().ToLowerInvariant(),
            location)
        {
        }
    }


    public enum KeywordTokenKind
    {
        True,
        False,
        Self,
        Super,
        Nil
    }
}
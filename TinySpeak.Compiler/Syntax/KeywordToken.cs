using System.Collections.Generic;
using Microsoft.Scripting;

namespace TinySpeak.Compiler.Syntax
{
    public class KeywordToken : IdOrKeywordToken
    {
        public KeywordToken(KeywordTokenKind name, SourceSpan location) : base(name.ToString().ToLowerInvariant(), location)
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
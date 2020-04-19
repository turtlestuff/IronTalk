using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Syntax
{
    public class IdOrKeywordToken : Token
    {
        public string Name { get; }

        public IdOrKeywordToken(string name, SourceSpan location) : base(location)
        {
            Name = name;
        }
    }
}
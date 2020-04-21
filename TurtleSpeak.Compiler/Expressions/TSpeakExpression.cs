using Microsoft.Scripting;

namespace TurtleSpeak.Compiler.Expressions
{
    public class TSpeakExpression
    {
        protected TSpeakExpression(SourceSpan location)
        {
            Location = location;
        }

        public SourceSpan Location { get; }
    }
}
using Microsoft.Scripting;

namespace IronTalk.Compiler.Expressions
{
    public class Node
    {
        public Node(SourceSpan location)
        {
            Location = location;
        }

        public SourceSpan Location { get; }

        public override string ToString() => $"{this.GetType().Name} @ {Location}";
    }
}
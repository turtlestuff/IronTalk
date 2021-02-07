using System.Numerics;
using Microsoft.Scripting;

namespace IronTalk.Compiler.Syntax
{
    public class NumberToken : LiteralToken
    {
        public NumberToken(double val, SourceSpan location) : base(val, location)
        {
        }

        public NumberToken(int val, SourceSpan location) : base(val, location)
        {
        }

        public NumberToken(long val, SourceSpan location) : base(val, location)
        {
        }

        public NumberToken(BigInteger val, SourceSpan location) : base(val, location)
        {
        }

        public override string ToString() => $"NumberToken: {Value.GetType().Name}:{Value}@{Location}";
    }
}
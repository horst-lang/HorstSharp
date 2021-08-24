using Horst.Values;

namespace Horst.Tokens
{
    public class Token
    {
        public TokenType Type { get; }
        public dynamic Value { get; }

        public Token(TokenType type, dynamic value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
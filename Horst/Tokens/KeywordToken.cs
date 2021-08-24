using Horst.Values;

namespace Horst.Tokens
{
    public class KeywordToken: Token
    {
        public KeywordToken(string value) : base(TokenType.Keyword, value)
        {
        }
    }
}
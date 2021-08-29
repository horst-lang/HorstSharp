namespace Horst.Tokens
{
    public class PunctuationToken: Token
    {
        public PunctuationToken(char value) : base(TokenType.Punctuation, value)
        {
        }
    }
}
namespace Horst.Tokens
{
    public class StringToken: Token
    {
        public StringToken(string value) : base(TokenType.String, value)
        {
        }
    }
}
namespace Horst.Tokens
{
    public class NumberToken : Token
    {
        public NumberToken(double value) : base(TokenType.Number, value)
        {
        }
    }
}
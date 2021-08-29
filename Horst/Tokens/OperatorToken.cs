namespace Horst.Tokens
{
    public class OperatorToken : Token
    {
        public OperatorToken(string value) : base(TokenType.Operator, value)
        {
        }
    }
}
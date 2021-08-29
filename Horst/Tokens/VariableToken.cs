namespace Horst.Tokens
{
    public class VariableToken: Token
    {
        public VariableToken(string value) : base(TokenType.Variable, value)
        {
        }
    }
}
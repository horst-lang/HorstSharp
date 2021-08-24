namespace Horst.Nodes
{
    public class BinaryExpressionNode: Node
    {
        public string Operator { get; }
        public Node Left { get; }
        public Node Right { get; }

        public BinaryExpressionNode(string @operator, Node left, Node right) : base(NodeType.BinaryExpression)
        {
            this.Operator = @operator;
            this.Left = left;
            this.Right = right;
        }
    }
}
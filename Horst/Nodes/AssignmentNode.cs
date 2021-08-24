namespace Horst.Nodes
{
    public class AssignmentNode: Node
    {
        public string Operator { get; }
        public Node Left { get; }
        public Node Right { get; }


        public AssignmentNode(string @operator, Node left, Node right) : base(NodeType.Assignment)
        {
            this.Operator = @operator;
            this.Left = left;
            this.Right = right;
        }
    }
}
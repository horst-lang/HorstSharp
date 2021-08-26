namespace Horst.Nodes
{
    public class FunctionCallNode: Node
    {
        public IdentifierNode Func { get; }
        public Node[] Args { get; }

        public FunctionCallNode(IdentifierNode func, Node[] args) : base(NodeType.FunctionCall)
        {
            this.Func = func;
            this.Args = args;
        }
    }
}
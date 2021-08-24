namespace Horst.Nodes
{
    public class FunctionCallNode: Node
    {
        public FunctionNode Func { get; }
        public Node[] Args { get; }

        public FunctionCallNode(FunctionNode func, Node[] args) : base(NodeType.FunctionCall)
        {
            this.Func = func;
            this.Args = args;
        }
    }
}
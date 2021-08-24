namespace Horst.Nodes
{
    public class FunctionNode: Node
    {
        public string[] Vars { get; }
        public Node Body { get; }

        public FunctionNode(string[] vars, Node body) : base(NodeType.Function)
        {
            this.Vars = vars;
            this.Body = body;
        }
    }
}
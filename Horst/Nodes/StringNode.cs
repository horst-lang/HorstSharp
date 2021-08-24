namespace Horst.Nodes
{
    public class StringNode: Node
    {
        public string Value { get; }

        public StringNode(string value) : base(NodeType.String)
        {
            this.Value = value;
        }
    }
}
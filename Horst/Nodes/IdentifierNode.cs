namespace Horst.Nodes
{
    public class IdentifierNode: Node
    {
        public string Value { get; }

        public IdentifierNode(string value) : base(NodeType.Identifier)
        {
            this.Value = value;
        }
    }
}
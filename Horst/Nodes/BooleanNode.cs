namespace Horst.Nodes
{
    public class BooleanNode: Node
    {
        public bool Value { get; }

        public BooleanNode(bool value) : base(NodeType.Boolean)
        {
            this.Value = value;
        }
    }
}
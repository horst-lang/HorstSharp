namespace Horst.Nodes
{
    public class SequenceNode: Node
    {
        public Node[] Sequence { get; }

        public SequenceNode(Node[] sequence) : base(NodeType.Sequence)
        {
            this.Sequence = sequence;
        }
    }
}
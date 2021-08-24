namespace Horst.Nodes
{
    public class Node
    {
        public NodeType Type { get; }

        public Node(NodeType type)
        {
            this.Type = type;
        }
    }
}
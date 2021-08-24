using Horst.Values;

namespace Horst.Nodes
{
    public class NumberNode: Node
    {
        public double Value { get; }

        public NumberNode(double value) : base(NodeType.Number)
        {
            this.Value = value;
        }
    }
}
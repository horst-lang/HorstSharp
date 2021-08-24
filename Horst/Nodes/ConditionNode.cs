namespace Horst.Nodes
{
    public class ConditionNode: Node
    {
        public Node Condition { get; }
        public Node Then { get; }
        public Node Else { get; set; }


        public ConditionNode(Node condition, Node then, Node @else = null) : base(NodeType.Condition)
        {
            this.Else = @else;
            this.Then = then;
            this.Condition = condition;
        }
    }
}
namespace IllusionScript.SDK.Nodes
{
    public class WhileNode : Node
    {
        public Node Condition;
        public Node Body;
        public bool ShouldReturnNull;

        public WhileNode(Node condition, Node body, bool shouldReturnNull) : base(condition.StartPos, body.EndPos)
        {
            Condition = condition;
            Body = body;
            ShouldReturnNull = shouldReturnNull;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
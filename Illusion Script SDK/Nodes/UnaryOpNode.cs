namespace IllusionScript.SDK.Nodes
{
    public class UnaryOpNode : Node
    {
        public Token OpToken;
        public Node Node;

        public UnaryOpNode(Token opToken, Node node) : base(opToken.StartPos, node.EndPos)
        {
            OpToken = opToken;
            Node = node;
        }

        public override string __repr__()
        {
            return $"{OpToken.__repr__()}, {Node.__repr__()}";
        }
    }
}
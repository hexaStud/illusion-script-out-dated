namespace IllusionScript.SDK.Nodes
{
    public class BinOpNode : Node
    {
        public Node LeftNode;
        public Token OpToken;
        public Node RightNode;


        public BinOpNode(Node leftNode, Token opToken, Node rightNode) : base(
            leftNode.StartPos, rightNode.EndPos)
        {
            LeftNode = leftNode;
            OpToken = opToken;
            RightNode = rightNode;
        }

        public override string __repr__()
        {
            return $"({LeftNode.__repr__()}, {OpToken.__repr__()}, ${RightNode.__repr__()})";
        }
    }
}
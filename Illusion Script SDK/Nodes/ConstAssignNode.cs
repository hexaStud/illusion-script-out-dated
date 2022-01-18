namespace IllusionScript.SDK.Nodes
{
    public class ConstAssignNode : Node
    {
        public Token Token;
        public Node Node;

        public ConstAssignNode(Token token, Node node) : base(token.StartPos, node.EndPos)
        {
            Token = token;
            Node = node;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
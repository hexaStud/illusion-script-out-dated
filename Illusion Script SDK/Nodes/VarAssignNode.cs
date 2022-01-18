namespace IllusionScript.SDK.Nodes
{
    public class VarAssignNode : Node
    {
        public Token Token;
        public Node Node;
        public bool DeclareNew;

        public VarAssignNode(Token token, Node node, bool declareNew) : base(token.StartPos, node.EndPos)
        {
            Token = token;
            Node = node;
            DeclareNew = declareNew;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
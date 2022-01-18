namespace IllusionScript.SDK.Nodes
{
    public class FieldNode : Node
    {
        public readonly Token ContextIsolation;
        public Token Token;
        public Node Node;

        public FieldNode(Token contextIsolation, Token token, Node node) : base(token.StartPos,
            node.EndPos)
        {
            ContextIsolation = contextIsolation;
            Token = token;
            Node = node;
        }


        public override string __repr__()
        {
            return "";
        }
    }
}
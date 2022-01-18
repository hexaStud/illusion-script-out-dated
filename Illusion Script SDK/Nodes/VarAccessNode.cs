namespace IllusionScript.SDK.Nodes
{
    public class VarAccessNode : Node
    {
        public Token Token;

        public VarAccessNode(Token token) : base(token.StartPos, token.EndPos)
        {
            Token = token;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
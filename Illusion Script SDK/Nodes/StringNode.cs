namespace IllusionScript.SDK.Nodes
{
    public class StringNode : Node
    {
        public Token Token;

        public StringNode(Token token) : base(token.StartPos, token.EndPos)
        {
            Token = token;
        }

        public override string __repr__()
        {
            return $"{Token.__repr__()}";
        }
    }
}
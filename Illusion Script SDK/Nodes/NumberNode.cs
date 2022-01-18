using IllusionScript.SDK;

namespace IllusionScript.SDK.Nodes
{
    public class NumberNode : Node
    {
        public Token Token;

        public NumberNode(Token token) : base(token.StartPos, token.EndPos)
        {
            Token = token;
        }

        public override string __repr__()
        {
            return $"{Token.__repr__()}";
        }
    }
}
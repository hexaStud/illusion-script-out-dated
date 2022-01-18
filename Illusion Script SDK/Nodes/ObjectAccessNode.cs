using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class ObjectAccessNode : Node
    {
        public List<Token> Tokens;

        public ObjectAccessNode(List<Token> tokens) : base(tokens[0].StartPos, tokens[^1].EndPos)
        {
            Tokens = tokens;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
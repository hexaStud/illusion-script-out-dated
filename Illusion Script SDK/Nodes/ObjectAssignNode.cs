using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class ObjectAssignNode : Node
    {
        public List<Token> Tokens;
        public Node Value;

        public ObjectAssignNode(List<Token> tokens, Node value) : base(tokens[0].StartPos, value.EndPos)
        {
            Tokens = tokens;
            Value = value;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
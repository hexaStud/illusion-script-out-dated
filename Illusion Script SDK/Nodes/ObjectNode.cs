using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class ObjectNode : Node
    {
        public Dictionary<Token, Node> Elements;

        public ObjectNode(Dictionary<Token, Node> elements, Position startPos, Position endPos) : base(startPos, endPos)
        {
            Elements = elements;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
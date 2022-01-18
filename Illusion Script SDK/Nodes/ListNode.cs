using System.Collections.Generic;
namespace IllusionScript.SDK.Nodes
{
    public class ListNode : Node
    {
        public List<Node> Elements;
        
        public ListNode(List<Node> elements, Position startPos, Position endPos) : base(startPos, endPos)
        {
            Elements = elements;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
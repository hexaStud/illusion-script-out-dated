using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class CallNode : Node
    {
        public Node Node;
        public List<Node> ArgNode;

        public CallNode(Node node, List<Node> argNode) : base(node.StartPos,
            (argNode.Count > 0) ? argNode[^1].EndPos : node.EndPos)
        {
            Node = node;
            ArgNode = argNode;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
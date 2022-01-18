using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class ObjectCallNode : Node
    {
        public Node Node;
        public List<Node> ArgNode;

        public ObjectCallNode(Node node, List<Node> argNode) : base(node.StartPos,
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
namespace IllusionScript.SDK.Nodes
{
    public class ReturnNode : Node
    {
        public Node Node;

        public ReturnNode(Node node, Position startPos, Position endPos) : base(startPos, endPos)
        {
            Node = node;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
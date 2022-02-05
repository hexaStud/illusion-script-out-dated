namespace IllusionScript.SDK.Nodes
{

    public abstract class HeaderNode : Node
    {
        public HeaderNode(Position startPos, Position endPos) : base(startPos, endPos)
        {
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
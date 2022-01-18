namespace IllusionScript.SDK.Nodes
{
    public class ExportNode : Node
    {
        public Node Func;
        
        public ExportNode(Position startPos, Position endPos, Node func) : base(startPos, endPos)
        {
            Func = func;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
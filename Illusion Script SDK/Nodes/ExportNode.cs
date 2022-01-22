namespace IllusionScript.SDK.Nodes
{
    public class ExportNode : Node
    {
        public Node Func;
        public string Name;

        public ExportNode(Position startPos, Position endPos, Node func, string name) : base(startPos, endPos)
        {
            Func = func;
            Name = name;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
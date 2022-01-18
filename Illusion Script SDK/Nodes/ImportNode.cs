namespace IllusionScript.SDK.Nodes
{
    public class ImportNode : HeaderNode
    {
        public Token Module;

        public ImportNode(Position startPos, Position endPos, Token module) : base(startPos, endPos)
        {
            Module = module;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
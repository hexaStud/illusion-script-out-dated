using IllusionScript.SDK.Bundler;

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

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"ReturnNode\", \"node\": {Node.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Node = ConvertNode(json.Get("node"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
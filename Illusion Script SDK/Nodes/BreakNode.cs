using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class BreakNode : Node
    {
        public BreakNode(Position startPos, Position endPos) : base(startPos, endPos)
        {
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"BreakNode\", \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
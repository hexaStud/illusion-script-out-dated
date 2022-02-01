using IllusionScript.SDK.Bundler;
using IllusionScript.SDK.Values;

namespace IllusionScript.SDK.Nodes
{

    public class NullNode : Node
    {
        public NullNode(Position startPos, Position endPos) : base(startPos, endPos)
        {
        }

        public override string __repr__()
        {
            return NumberValue.Null.Value.GetAsString();
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"NullNode\", \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
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
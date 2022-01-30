using System;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ContinueNode : Node
    {
        public ContinueNode(Position startPos, Position endPos) : base(startPos, endPos)
        {
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"ContinueNode\", \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
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
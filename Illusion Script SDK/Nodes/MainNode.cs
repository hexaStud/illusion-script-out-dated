using System;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{

    public class MainNode : Node
    {
        public MainNode() : base(Position.Empty(), Position.Empty())
        {
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"MainNode\", \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
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
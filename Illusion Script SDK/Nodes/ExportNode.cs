using System;
using IllusionScript.SDK.Bundler;

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

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"ExportNode\", \"func\": {Func.__bundle__()}, \"name\": \"{Name}\", \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Func = ConvertNode(json.Get("func"));
            Name = json.GetAsText("name");

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
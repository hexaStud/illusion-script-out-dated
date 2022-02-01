using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ListNode : Node
    {
        public List<Node> Elements;

        public ListNode(List<Node> elements, Position startPos, Position endPos) : base(startPos, endPos)
        {
            Elements = elements;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            var args = "[";
            var first = true;
            foreach (var node in Elements)
            {
                if (!first) args += ",";

                args += node.__bundle__();
                first = false;
            }

            args += "]";

            return "{" +
                   $"\"type\": \"ListNode\", \"elements\": {args}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Elements = new List<Node>();
            var elements = json.Get("elements");
            for (var i = 0; i < Json.Length(elements); i++) Elements.Add(ConvertNode(elements.Get(i.ToString())));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
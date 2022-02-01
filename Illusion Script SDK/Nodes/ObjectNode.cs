using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ObjectNode : Node
    {
        public Dictionary<Token, Node> Elements;

        public ObjectNode(Dictionary<Token, Node> elements, Position startPos, Position endPos) : base(startPos, endPos)
        {
            Elements = elements;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            var first = true;
            var elements = "[";

            foreach (var elementsKey in Elements.Keys)
            {
                if (!first) elements += ",";

                elements += "{" +
                            $"\"key\", {elementsKey.__bundle__()}, \"value\": {Elements[elementsKey].__bundle__()}" +
                            "}";

                first = false;
            }

            return "{" +
                   $"\"type\": \"ObjectNode\", \"elements\": {elements}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Elements = new Dictionary<Token, Node>();
            var elements = json.Get("elements");
            for (var i = 0; i < Json.Length(elements); i++)
                Elements[Token.Convert(elements.Get(i.ToString()).Get("key"))] =
                    ConvertNode(elements.Get(i.ToString()).Get("value"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
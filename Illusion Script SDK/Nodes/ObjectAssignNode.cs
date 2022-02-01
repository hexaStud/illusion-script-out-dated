using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ObjectAssignNode : Node
    {
        public List<Token> Tokens;
        public Node Value;

        public ObjectAssignNode(List<Token> tokens, Node value) : base(tokens[0].StartPos, value.EndPos)
        {
            Tokens = tokens;
            Value = value;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            var args = "[";
            var first = true;
            foreach (var node in Tokens)
            {
                if (!first) args += ",";

                args += node.__bundle__();
                first = false;
            }

            args += "]";

            return "{" +
                   $"\"type\": \"ObjectAssignNode\", \"tokens\": {args}, \"value\": {Value.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Tokens = new List<Token>();
            var tokens = json.Get("tokens");
            for (var i = 0; i < Json.Length(tokens); i++) Tokens.Add(Token.Convert(tokens.Get(i.ToString())));

            Value = ConvertNode(json.Get("value"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
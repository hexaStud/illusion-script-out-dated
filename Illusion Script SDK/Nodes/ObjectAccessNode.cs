using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ObjectAccessNode : Node
    {
        public List<Token> Tokens;

        public ObjectAccessNode(List<Token> tokens) : base(tokens[0].StartPos, tokens[^1].EndPos)
        {
            Tokens = tokens;
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
                   $"\"type\": \"ObjectAccessNode\", \"tokens\": {args}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Tokens = new List<Token>();
            var tokens = json.Get("tokens");

            for (var i = 0; i < Json.Length(tokens); i++) Tokens.Add(Token.Convert(tokens.Get(i.ToString())));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
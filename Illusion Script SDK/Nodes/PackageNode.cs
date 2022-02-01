using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    internal class PackageNode : HeaderNode
    {
        public string FileName;
        public List<Token> Names;

        public PackageNode(List<Token> names) : base(names[0].StartPos, names[^1].EndPos)
        {
            Names = names;
            FileName = names[0].StartPos.FileName;
        }

        public override string __bundle__()
        {
            var args = "[";
            var first = true;
            foreach (var node in Names)
            {
                if (!first) args += ",";

                args += node.__bundle__();
                first = false;
            }

            args += "]";

            return "{" +
                   $"\"type\": \"PackageNode\", \"fileName\": \"{FileName}\", \"names\": {args}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            FileName = json.GetAsText("fileName");
            Names = new List<Token>();
            var args = json.Get("names");
            for (var i = 0; i < Json.Length(args); i++) Names.Add(Token.Convert(args.Get(i.ToString())));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
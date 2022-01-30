using System;
using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ClassNode : Node
    {
        public Token Name;
        public List<Node> Fields;
        public List<Node> StaticFields;
        public Token Extends;

        public ClassNode(Token name, List<Node> fields, List<Node> staticFields, Token extends) : base(name.StartPos,
            fields.Count == 0 ? name.EndPos : fields[^1].EndPos)
        {
            Name = name;
            Fields = fields;
            StaticFields = staticFields;
            Extends = extends;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            string fields = "[";
            bool first = true;
            foreach (Node node in Fields)
            {
                if (!first)
                {
                    fields += ",";
                }

                fields += node.__bundle__();
                first = false;
            }

            fields += "]";

            string staticFields = "[";
            first = true;
            foreach (Node node in StaticFields)
            {
                if (!first)
                {
                    staticFields += ",";
                }

                staticFields += node.__bundle__();
                first = false;
            }

            staticFields += "]";

            string extends = Extends == default(Token) ? "false" : Extends.__bundle__();

            return "{" +
                   $"\"type\": \"ClassNode\", \"name\": {Name.__bundle__()}, \"fields\": {fields}, \"staticFields\": {staticFields}, \"extends\": {extends}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Name = Token.Convert(json.Get("name"));
            Fields = new List<Node>();

            Json fields = json.Get("fields");
            for (int i = 0; i < Json.Length(fields); i++)
            {
                Fields.Add(ConvertNode(fields.Get(i.ToString())));
            }
            
            StaticFields = new List<Node>();

            Json staticFields = json.Get("fields");
            for (int i = 0; i < Json.Length(staticFields); i++)
            {
                Fields.Add(ConvertNode(staticFields.Get(i.ToString())));
            }

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
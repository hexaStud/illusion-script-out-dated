using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ClassNode : Node
    {
        public Token Extends;
        public List<Node> Fields;
        public Token Name;
        public List<Node> StaticFields;

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
            var fields = "[";
            var first = true;
            foreach (var node in Fields)
            {
                if (!first) fields += ",";

                fields += node.__bundle__();
                first = false;
            }

            fields += "]";

            var staticFields = "[";
            first = true;
            foreach (var node in StaticFields)
            {
                if (!first) staticFields += ",";

                staticFields += node.__bundle__();
                first = false;
            }

            staticFields += "]";

            var extends = Extends == default(Token) ? "false" : Extends.__bundle__();

            return "{" +
                   $"\"type\": \"ClassNode\", \"name\": {Name.__bundle__()}, \"fields\": {fields}, \"staticFields\": {staticFields}, \"extends\": {extends}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Name = Token.Convert(json.Get("name"));
            Fields = new List<Node>();

            var fields = json.Get("fields");
            for (var i = 0; i < Json.Length(fields); i++) Fields.Add(ConvertNode(fields.Get(i.ToString())));

            StaticFields = new List<Node>();

            var staticFields = json.Get("fields");
            for (var i = 0; i < Json.Length(staticFields); i++) Fields.Add(ConvertNode(staticFields.Get(i.ToString())));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
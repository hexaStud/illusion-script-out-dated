using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class ClassNode : Node
    {
        public readonly Token Name;
        public readonly List<Node> Fields;
        public readonly List<Node> StaticFields;
        public readonly Token Extends;

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
    }
}
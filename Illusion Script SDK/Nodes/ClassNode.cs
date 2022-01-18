using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class ClassNode : Node
    {
        public Token Name;
        public List<Node> Fields;

        public ClassNode(Token name, List<Node> fields) : base(name.StartPos,
            fields.Count == 0 ? name.EndPos : fields[^1].EndPos)
        {
            Name = name;
            Fields = fields;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
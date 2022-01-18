using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class ClassConstructorNode : Node
    {
        public Node ClassName;
        public List<Node> ConstructArgs;

        public ClassConstructorNode(Node className, List<Node> constructArgs) : base(className.StartPos,
            constructArgs.Count == 0 ? className.EndPos : constructArgs[^1].EndPos)
        {
            ClassName = className;
            ConstructArgs = constructArgs;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
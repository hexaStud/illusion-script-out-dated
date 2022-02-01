using System.Collections.Generic;

namespace IllusionScript.SDK
{
    public class ParserValue
    {
        public const int ListType = 0;
        public const int NodeType = 1;
        private readonly List<Node> List;
        private readonly Node Node;

        private readonly int Type;

        public ParserValue(Node node)
        {
            Node = node;
            Type = NodeType;
        }

        public ParserValue(List<Node> node)
        {
            List = node;
            Type = ListType;
        }

        public (Node node, List<Node>nodes, int mode) Get()
        {
            if (Type == ListType)
                return (default, List, Type);
            return (Node, default, Type);
        }
    }
}
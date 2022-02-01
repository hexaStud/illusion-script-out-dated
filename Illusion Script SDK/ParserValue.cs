using System.Collections.Generic;

namespace IllusionScript.SDK
{
    public class ParserValue
    {
        public const int ListType = 0;
        public const int NodeType = 1;

        private int Type;
        private List<Node> List;
        private Node Node;

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
            {
                return (default, List, Type);
            }
            else
            {
                return (Node, default, Type);
            }
        }
    }
}
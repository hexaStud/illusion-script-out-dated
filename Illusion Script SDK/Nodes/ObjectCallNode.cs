using System;
using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ObjectCallNode : Node
    {
        public Node Node;
        public List<Node> ArgNode;

        public ObjectCallNode(Node node, List<Node> argNode) : base(node.StartPos,
            (argNode.Count > 0) ? argNode[^1].EndPos : node.EndPos)
        {
            Node = node;
            ArgNode = argNode;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            string args = "[";
            bool first = true;
            foreach (Node node in ArgNode)
            {
                if (!first)
                {
                    args += ",";
                }

                args += node.__bundle__();
                first = false;
            }

            args += "]";

            return "{" +
                   $"\"type\": \"ObjectCallNode\", \"argNode\": {args}, \"node\": {Node.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            ArgNode = new List<Node>();
            Json args = json.Get("argNode");
            for (int i = 0; i < Json.Length(args); i++)
            {
                ArgNode.Add(ConvertNode(args.Get(i.ToString())));
            }

            Node = ConvertNode(json.Get("node"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
using System;
using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class CallNode : Node
    {
        public Node Node;
        public List<Node> ArgNode;

        public CallNode(Node node, List<Node> argNode) : base(node.StartPos,
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
                   $"\"type\": \"CallNode\", \"node\": {Node.__bundle__()}, \"args\": {args}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Node = ConvertNode(json.Get("node"));
            ArgNode = new List<Node>();

            Json args = json.Get("args");

            for (int i = 0; i < Json.Length(args); i++)
            {
                ArgNode.Add(ConvertNode(args.Get(i.ToString())));
            }


            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
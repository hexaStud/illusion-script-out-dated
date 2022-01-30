using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

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

        public override string __bundle__()
        {
            string args = "[";
            bool first = true;
            foreach (Node node in ConstructArgs)
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
                   $"\"type\": \"ClassConstructorNode\", \"className\": {ClassName.__bundle__()}, \"constructArgs\": {args}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            ClassName = ConvertNode(json.Get("className"));
            ConstructArgs = new List<Node>();

            Json args = json.Get("constructArgs");

            for (int i = 0; i < Json.Length(args); i++)
            {
                ConstructArgs.Add(ConvertNode(args.Get(i.ToString())));
            }

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
using System;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ElseCaseNode : Node
    {
        public Node Statements;
        public bool Bool;

        public ElseCaseNode(Node statements, bool bl) : base(statements.StartPos, statements.EndPos)
        {
            Statements = statements;
            Bool = bl;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"ElseCaseNode\", \"statements\": {Statements.__bundle__()}, \"bool\": {Bool.ToString().ToLower()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Statements = ConvertNode(json.Get("statements"));
            Bool = json.GetAsBool("bool");

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
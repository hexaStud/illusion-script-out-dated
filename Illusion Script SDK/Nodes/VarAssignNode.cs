using System;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class VarAssignNode : Node
    {
        public Token Token;
        public Node Node;
        public bool DeclareNew;

        public VarAssignNode(Token token, Node node, bool declareNew) : base(token.StartPos, node.EndPos)
        {
            Token = token;
            Node = node;
            DeclareNew = declareNew;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"VarAssignNode\", \"token\": {Token.__bundle__()}, \"node\": {Node.__bundle__()}, \"declareNew\": {DeclareNew.ToString().ToLower()} \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Token = Token.Convert(json.Get("token"));
            Node = ConvertNode(json.Get("node"));
            DeclareNew = json.GetAsBool("declareNew");

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
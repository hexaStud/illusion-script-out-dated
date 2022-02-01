using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class FieldNode : Node
    {
        public Token ContextIsolation;
        public Node Node;
        public Token Token;

        public FieldNode(Token contextIsolation, Token token, Node node) : base(token.StartPos,
            node.EndPos)
        {
            ContextIsolation = contextIsolation;
            Token = token;
            Node = node;
        }


        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"FieldNode\", \"contextIsolation\": {ContextIsolation.__bundle__()}, \"token\": {Token.__bundle__()}, \"node\": {Node.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            ContextIsolation = Token.Convert(json.Get("contextIsolation"));
            Token = Token.Convert(json.Get("token"));
            Node = ConvertNode(json.Get("node"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
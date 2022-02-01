using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ConstAssignNode : Node
    {
        public Token Token;
        public Node Node;

        public ConstAssignNode(Token token, Node node) : base(token.StartPos, node.EndPos)
        {
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
                   $"\"type\": \"ConstAssignNode\", \"token\": {Token.__bundle__()}, \"node\": {Node.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Token = Token.Convert(json.Get("token"));
            Node = ConvertNode(json.Get("node"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
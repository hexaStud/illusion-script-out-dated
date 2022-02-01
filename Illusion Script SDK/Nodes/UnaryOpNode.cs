using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class UnaryOpNode : Node
    {
        public Node Node;
        public Token OpToken;

        public UnaryOpNode(Token opToken, Node node) : base(opToken.StartPos, node.EndPos)
        {
            OpToken = opToken;
            Node = node;
        }

        public override string __repr__()
        {
            return $"{OpToken.__repr__()}, {Node.__repr__()}";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"UnaryOpNode\", \"opToken\": {OpToken.__bundle__()}, \"node\": {Node.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            OpToken = Token.Convert(json.Get("opToken"));
            Node = ConvertNode(json.Get("node"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
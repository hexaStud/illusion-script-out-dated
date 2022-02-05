using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class BinOpNode : Node
    {
        public Node LeftNode;
        public Token OpToken;
        public Node RightNode;


        public BinOpNode(Node leftNode, Token opToken, Node rightNode) : base(
            leftNode.StartPos, rightNode.EndPos)
        {
            LeftNode = leftNode;
            OpToken = opToken;
            RightNode = rightNode;
        }

        public override string __repr__()
        {
            return $"({LeftNode.__repr__()}, {OpToken.__repr__()}, ${RightNode.__repr__()})";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"BinOpNode\",\"left\": {LeftNode.__bundle__()},\"token\": {OpToken.__bundle__()}, \"right\": {RightNode.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            LeftNode = ConvertNode(json.Get("left"));
            RightNode = ConvertNode(json.Get("right"));
            OpToken = Token.Convert(json.Get("token"));
            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
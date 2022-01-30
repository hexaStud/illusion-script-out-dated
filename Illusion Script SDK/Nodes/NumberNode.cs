using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class NumberNode : Node
    {
        public Token Token;

        public NumberNode(Token token) : base(token.StartPos, token.EndPos)
        {
            Token = token;
        }

        public override string __repr__()
        {
            return $"{Token.__repr__()}";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"NumberNode\", \"token\": {Token.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Token = Token.Convert(json.Get("token"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
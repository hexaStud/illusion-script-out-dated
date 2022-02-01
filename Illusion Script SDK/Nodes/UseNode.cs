using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    internal class UseNode : HeaderNode
    {
        public Token Module;

        public UseNode(Position startPos, Position endPos, Token module) : base(startPos, endPos)
        {
            Module = module;
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"UseNode\", \"module\": {Module.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Module = Token.Convert(json.Get("module"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
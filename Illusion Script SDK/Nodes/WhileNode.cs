using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class WhileNode : Node
    {
        public Node Condition;
        public Node Body;
        public bool ShouldReturnNull;

        public WhileNode(Node condition, Node body, bool shouldReturnNull) : base(condition.StartPos, body.EndPos)
        {
            Condition = condition;
            Body = body;
            ShouldReturnNull = shouldReturnNull;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            return "{" +
                   $"\"type\": \"WhileNode\", \"condition\": {Condition.__bundle__()}, \"body\": {Body.__bundle__()}, \"shouldReturnNull\": {ShouldReturnNull.ToString().ToLower()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Condition = ConvertNode(json.Get("condition"));
            Body = ConvertNode(json.Get("body"));
            ShouldReturnNull = json.GetAsBool("shouldReturnNull");

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
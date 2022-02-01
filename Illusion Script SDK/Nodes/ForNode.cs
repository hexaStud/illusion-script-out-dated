using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class ForNode : Node
    {
        public Node Body;
        public Node EndValue;
        public bool ShouldReturnNull;
        public Node StartValue;
        public Node StepValue;
        public Token VarName;

        public ForNode(Token varName, Node startValue, Node endValue, Node stepValue, Node body,
            bool shouldReturnNull) : base(varName.StartPos, body.EndPos)
        {
            VarName = varName;
            StartValue = startValue;
            EndValue = endValue;
            StepValue = stepValue;
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
                   $"\"type\": \"ForNode\", \"varName\": {VarName.__bundle__()}, \"startValue\": {StartValue.__bundle__()}, \"endValue\": {EndValue.__bundle__()}, \"stepValue\": {StepValue.__bundle__()}, \"body\": {Body.__bundle__()}, \"shouldReturnNull\": {ShouldReturnNull.ToString().ToLower()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            VarName = Token.Convert(json.Get("varName"));
            StartValue = ConvertNode(json.Get("startValue"));
            EndValue = ConvertNode(json.Get("endValue"));
            StepValue = ConvertNode(json.Get("stepValue"));
            Body = ConvertNode(json.Get("body"));
            ShouldReturnNull = json.GetAsBool("shouldReturnNull");

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
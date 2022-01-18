namespace IllusionScript.SDK.Nodes
{
    public class ForNode : Node
    {
        public Token VarName;
        public Node StartValue;
        public Node EndValue;
        public Node StepValue;
        public Node Body;
        public bool ShouldReturnNull;

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
    }
}
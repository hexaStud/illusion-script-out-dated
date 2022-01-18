using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class MethodDefineNode : Node
    {
        public Token VarName;
        public List<Token> ArgName;
        public Node Body;
        public bool ShouldAutoReturn;

        public MethodDefineNode(Token varName, List<Token> argName, Node body, bool shouldAutoReturn) : base(
            varName != default(Token) ? varName.StartPos : (argName.Count > 0) ? argName[0].StartPos : body.StartPos,
            body.EndPos)
        {
            VarName = varName;
            ArgName = argName;
            Body = body;
            ShouldAutoReturn = shouldAutoReturn;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
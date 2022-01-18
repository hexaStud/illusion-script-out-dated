using System.Collections.Generic;

namespace IllusionScript.SDK.Nodes
{
    public class MethodDefineNode : Node
    {
        public readonly Token ContextIsolation;
        public readonly Token VarName;
        public readonly List<Token> ArgName;
        public readonly Node Body;
        public readonly bool ShouldAutoReturn;

        public MethodDefineNode(Token contextIsolation, Token varName, List<Token> argName, Node body,
            bool shouldAutoReturn) : base(
            varName != default(Token) ? varName.StartPos : (argName.Count > 0) ? argName[0].StartPos : body.StartPos,
            body.EndPos)
        {
            ContextIsolation = contextIsolation;
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
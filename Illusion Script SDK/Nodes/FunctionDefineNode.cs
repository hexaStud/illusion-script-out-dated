using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class FunctionDefineNode : Node
    {
        public List<Token> ArgName;
        public Node Body;
        public bool ShouldAutoReturn;
        public Token VarName;

        public FunctionDefineNode(Token varName, List<Token> argName, Node body, bool shouldAutoReturn) : base(
            varName != default(Token) ? varName.StartPos : argName.Count > 0 ? argName[0].StartPos : body.StartPos,
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

        public override string __bundle__()
        {
            var args = "[";
            var first = true;
            foreach (var node in ArgName)
            {
                if (!first) args += ",";

                args += node.__bundle__();
                first = false;
            }

            args += "]";
            return "{" +
                   $"\"type\": \"FunctionDefineNode\", \"varName\": {VarName.__bundle__()}, \"argName\": {args}, \"body\": {Body.__bundle__()}, \"shouldAutoReturn\": {ShouldAutoReturn.ToString().ToLower()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            VarName = Token.Convert(json.Get("varName"));
            ArgName = new List<Token>();

            var args = json.Get("argName");

            for (var i = 0; i < Json.Length(args); i++) ArgName.Add(Token.Convert(args.Get(i.ToString())));

            Body = ConvertNode(json.Get("body"));
            ShouldAutoReturn = json.GetAsBool("shouldAutoReturn");

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
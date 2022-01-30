using System;
using System.Collections.Generic;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class MethodDefineNode : Node
    {
        public Token ContextIsolation;
        public Token VarName;
        public List<Token> ArgName;
        public Node Body;
        public bool ShouldAutoReturn;

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

        public override string __bundle__()
        {
            string args = "[";
            bool first = true;
            foreach (Token node in ArgName)
            {
                if (!first)
                {
                    args += ",";
                }

                args += node.__bundle__();
                first = false;
            }

            args += "]";

            return "{" +
                   $"\"type\": \"MethodDefineNode\", \"contextIsolation\": {ContextIsolation.__bundle__()}, \"varName\": {VarName.__bundle__()}, \"argName\": {args}, \"body\": {Body.__bundle__()}, \"shouldAutoReturn\": {ShouldAutoReturn.ToString().ToLower()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            ContextIsolation = Token.Convert(json.Get("contextIsolation"));
            VarName = Token.Convert(json.Get("varName"));
            ArgName = new List<Token>();

            Json args = json.Get("argName");
            for (int i = 0; i < Json.Length(args); i++)
            {
                ArgName.Add(Token.Convert(args.Get(i.ToString())));
            }
            
            Body = ConvertNode(json.Get("body"));
            ShouldAutoReturn = json.GetAsBool("shouldAutoReturn");

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
using System.Collections.Generic;
using IllusionScript.SDK.Bundler;
using IllusionScript.SDK.Nodes.Assets;

namespace IllusionScript.SDK.Nodes
{
    public class HeadIfNode : HeaderNode
    {
        public List<IfCase> Cases;
        public ElseCaseNode ElseCase;

        public HeadIfNode(List<IfCase> cases, Node elseCase) : base(cases[0].Statements.StartPos,
            elseCase == default(Node) ? cases[^1].Statements.EndPos : elseCase.EndPos)
        {
            Cases = cases;
            ElseCase = (ElseCaseNode)elseCase;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            var cases = "[";
            var first = true;
            foreach (var node in Cases)
            {
                if (!first) cases += ",";

                cases += node.__bundle__();
                first = false;
            }

            cases += "]";

            return "{" +
                   $"\"type\": \"HeadIfNode\", \"cases\": {cases}, \"elseCase\": {ElseCase.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            var cases = json.Get("cases");

            Cases = new List<IfCase>();
            for (var i = 0; i < Json.Length(cases); i++) Cases.Add(new IfCase().__unbundle__(cases.Get(i.ToString())));

            ElseCase = (ElseCaseNode)ConvertNode(json.Get("elseCase"));

            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
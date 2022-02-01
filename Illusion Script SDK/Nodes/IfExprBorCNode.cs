using System.Collections.Generic;
using IllusionScript.SDK.Bundler;
using IllusionScript.SDK.Nodes.Assets;

namespace IllusionScript.SDK.Nodes
{

    public class IfExprBorCNode : Node
    {
        public List<IfCase> Cases;
        public Node ElseCase;

        public IfExprBorCNode(List<IfCase> cases, Node elseCase) : base(Position.Empty(), Position.Empty())
        {
            Cases = cases;
            ElseCase = elseCase;
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            string cases = "[";
            bool first = true;
            foreach (IfCase node in Cases)
            {
                if (!first)
                {
                    cases += ",";
                }

                cases += node.__bundle__();
                first = false;
            }

            cases += "]";

            return "{" +
                   $"\"type\": \"IfExprBorCNode\", \"cases\": {cases}, \"elseCase\": {ElseCase.__bundle__()}, \"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public override Node __unbundle__(Json json)
        {
            Json cases = json.Get("cases");

            Cases = new List<IfCase>();
            for (int i = 0; i < Json.Length(cases); i++)
            {
                Cases.Add(new IfCase().__unbundle__(cases.Get(i.ToString())));
            }

            ElseCase = (ElseCaseNode)ConvertNode(json.Get("elseCase"));
            
            StartPos = Position.Convert(json.Get("startPos"));
            EndPos = Position.Convert(json.Get("endPos"));
            return this;
        }
    }
}
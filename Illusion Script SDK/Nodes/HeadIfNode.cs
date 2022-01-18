﻿using System.Collections.Generic;
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
            ElseCase = (ElseCaseNode) elseCase;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
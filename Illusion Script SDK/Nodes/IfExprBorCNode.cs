using System.Collections.Generic;
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
    }
}
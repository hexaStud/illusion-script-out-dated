namespace IllusionScript.SDK.Nodes
{
    public class ElseCaseNode : Node
    {
        public Node Statements;
        public bool Bool;

        public ElseCaseNode(Node statements, bool bl) : base(statements.StartPos, statements.EndPos)
        {
            Statements = statements;
            Bool = bl;
        }

        public override string __repr__()
        {
            return "";
        }
    }
}
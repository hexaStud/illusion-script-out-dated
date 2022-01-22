using IllusionScript.SDK.Values;

namespace IllusionScript.SDK.Nodes
{
    public class NullNode : Node
    {
        public NullNode(Position startPos, Position endPos) : base(startPos, endPos)
        {
        }

        public override string __repr__()
        {
            return NumberValue.Null.Value.GetAsString();
        }
    }
}
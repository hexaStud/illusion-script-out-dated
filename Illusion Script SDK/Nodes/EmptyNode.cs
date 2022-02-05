using System;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK.Nodes
{
    public class EmptyNode : Node
    {
        public EmptyNode() : base(Position.Empty(), Position.Empty())
        {
        }

        public override string __repr__()
        {
            return "";
        }

        public override string __bundle__()
        {
            throw new Exception("Cannot bundle empty node");
        }

        public override Node __unbundle__(Json json)
        {
            throw new Exception("Cannot unbundle empty node");
        }
    }
}
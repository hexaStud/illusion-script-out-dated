namespace IllusionScript.SDK
{
    public abstract class Node
    {
        public Position StartPos;
        public Position EndPos;

        protected Node(Position startPos, Position endPos)
        {
            StartPos = startPos;
            EndPos = endPos;
        }

        public abstract string __repr__();
    }
}
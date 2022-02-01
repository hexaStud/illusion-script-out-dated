namespace IllusionScript.SDK
{
    public abstract class Error
    {
        protected string Name;
        protected string Details;
        protected Position StartPos;
        protected Position EndPos;

        protected Error(string name, string details, Position startPos, Position endPos)
        {
            Name = name;
            Details = details;
            StartPos = startPos;
            EndPos = endPos;
        }

        public override string ToString()
        {
            string result = $"{Name}: {Details}\n";
            result += $"File {StartPos.FileName}, line {StartPos.Ln + 1}\n\n";
            // result += StringWithArrows();
            return result;
        }
    }
}
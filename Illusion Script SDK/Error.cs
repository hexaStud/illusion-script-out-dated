namespace IllusionScript.SDK
{
    public abstract class Error
    {
        protected string Details;
        protected Position EndPos;
        protected string Name;
        protected Position StartPos;

        protected Error(string name, string details, Position startPos, Position endPos)
        {
            Name = name;
            Details = details;
            StartPos = startPos;
            EndPos = endPos;
        }

        public override string ToString()
        {
            var result = $"{Name}: {Details}\n";
            result += $"File {StartPos.FileName}, line {StartPos.Ln + 1}\n\n";
            // result += StringWithArrows();
            return result;
        }
    }
}
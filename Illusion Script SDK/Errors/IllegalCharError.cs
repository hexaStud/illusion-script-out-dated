namespace IllusionScript.SDK.Errors
{
    public class IllegalCharError : Error
    {
        public IllegalCharError(string details, Position startPos, Position endPos) : base("Illegal Character", details,
            startPos, endPos)
        {
        }
    }
}
namespace IllusionScript.SDK.Errors
{
    public class UnexpectedCharError : Error
    {
        public UnexpectedCharError(string details, Position startPos, Position endPos) : base("Unexpected Char Error",
            details, startPos, endPos)
        {
        }
    }
}
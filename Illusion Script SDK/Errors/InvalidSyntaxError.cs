namespace IllusionScript.SDK.Errors
{
    public class InvalidSyntaxError : Error
    {
        public InvalidSyntaxError(string details, Position startPos, Position endPos) : base("Invalid Syntax Error",
            details, startPos, endPos)
        {
        }
    }
}
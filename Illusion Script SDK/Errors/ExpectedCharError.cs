namespace IllusionScript.SDK.Errors
{
    public class ExpectedCharError : Error
    {
        public ExpectedCharError(string details, Position startPos, Position endPos) : base("Expected Character",
            details, startPos, endPos)
        {
        }
    }
}
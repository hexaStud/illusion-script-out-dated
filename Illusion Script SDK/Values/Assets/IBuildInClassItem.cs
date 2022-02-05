namespace IllusionScript.SDK.Values.Assets
{
    public interface IBuildInClassItem
    {
        public string Name { get; }
        public Token Isolation { get; }
        
        public static readonly Token PUBLIC = new Token(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.PUBLIC));
        public static readonly Token PRIVATE = new Token(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.PRIVATE));
    }
}
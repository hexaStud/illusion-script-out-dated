namespace IllusionScript.SDK
{
    public class Token
    {
        public string Type;
        public TokenValue Value;
        public Position StartPos;
        public Position EndPos;

        public Token(string type)
        {
            Type = type;
        }

        public Token(string type, TokenValue value)
        {
            Type = type;
            Value = value;
        }

        public Token(string type, TokenValue value, Position startPos)
        {
            Type = type;
            Value = value;
            StartPos = startPos;
            EndPos = startPos.Copy();
            EndPos.Advance();
        }

        public Token(string type, TokenValue value, Position startPos, Position endPos)
        {
            Type = type;
            Value = value;
            StartPos = startPos;
            EndPos = endPos;
        }

        public bool Matches(string type, TokenValue value)
        {
            return Type == type && Value.Matches(value);
        }

        public string __repr__()
        {
            if (Value == default(TokenValue))
            {
                return $"{Type}";
            }
            else
            {
                return $"{Type}:{Value.Value}";
            }
        }
    }
}
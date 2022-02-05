using IllusionScript.SDK.Bundler;

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

        public string __bundle__()
        {
            string value = "";
            if (Value != default(TokenValue))
            {
                value = $"\"value\": {Value.__bundle__()}, ";
            }

            return "{" +
                   $"\"type\": \"Token\", \"tt\": \"{Type}\", {value}\"startPos\": {StartPos.__bundle__()}, \"endPos\": {EndPos.__bundle__()}" +
                   "}";
        }

        public static Token Convert(Json json)
        {
            Token tok = Empty();
            tok.Type = json.GetAsText("tt");
            tok.Value = Json.KeyExists(json, "value") ? TokenValue.Convert(json.Get("value")) : default;
            tok.StartPos = Position.Convert(json.Get("startPos"));
            tok.EndPos = Position.Convert(json.Get("endPos"));
            return tok;
        }

        public static Token Empty()
        {
            return new Token("", new TokenValue(typeof(string), ""), Position.Empty(), Position.Empty());
        }
    }
}
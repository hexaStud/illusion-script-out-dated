using System;
using IllusionScript.SDK.Bundler;

namespace IllusionScript.SDK
{
    public class TokenValue
    {
        public Type Type;
        public string Value;

        public TokenValue(Type type, string value)
        {
            Type = type;
            Value = value;
        }

        public bool Matches(TokenValue other)
        {
            return Type == other.Type && Value == other.Value;
        }

        public bool IsInt()
        {
            return Type == typeof(int);
        }

        public bool IsFloat()
        {
            return Type == typeof(float);
        }

        public string GetAsString()
        {
            return Value;
        }

        public int GetAsInt()
        {
            if (!int.TryParse(Value, out int value))
            {
                throw new Exception($"Cannot convert '{Value}' to int");
            }

            return value;
        }

        public float GetAsFloat()
        {
            if (Type == typeof(int))
            {
                return (float)GetAsInt();
            }
            
            if (!float.TryParse(Value, out float value))
            {
                throw new Exception($"Cannot convert '{Value}' to float");
            }

            return value;
        }

        public string __bundle__()
        {
            return "{" + $"\"type\": \"{Type.Name}\", \"value\": \"{Value}\"" + "}";
        }

        public static TokenValue Convert(Json json)
        {
            TokenValue tokenValue = new TokenValue(default, default);
            Type type;
            switch (json.GetAsText("type"))
            {
                case "String":
                    type = typeof(string);
                    break;
                case "Int32":
                    type = typeof(int);
                    break;
                case "Single":
                    type = typeof(float);
                    break;
                case "Boolean":
                    type = typeof(bool);
                    break;
                default:
                    throw new Exception($"Undefined type '{json.GetAsText("type")}'");
            }

            tokenValue.Type = type;
            tokenValue.Value = json.GetAsText("value");
            return tokenValue;
        }
    }
}
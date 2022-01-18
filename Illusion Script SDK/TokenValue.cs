using System;

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
            if (!float.TryParse(Value, out float value))
            {
                throw new Exception($"Cannot convert '{Value}' to float");
            }

            return value;
        }
    }
}
using System;
using IllusionScript.SDK.Extensions;


namespace IllusionScript.SDK.Values
{
    public class StringValue : Value
    {
        public TokenValue Value;

        public StringValue(TokenValue value)
        {
            Value = value;
        }

        public override Tuple<Error, Value> AddedTo(Value other)
        {
            if (other.GetType() == typeof(StringValue))
            {
                StringValue otherString = (StringValue) other;

                return new Tuple<Error, Value>(default,
                    new StringValue(new TokenValue(typeof(string), Value.Value + otherString.Value.Value))
                        .SetContext(Context));
            }

            return base.AddedTo(other);
        }

        public override Tuple<Error, Value> MultedBy(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue numberValue = (NumberValue) other;
                return new Tuple<Error, Value>(default,
                    new StringValue(new TokenValue(typeof(string), Value.Value.Repeat(numberValue.Value.GetAsInt())))
                        .SetContext(Context));
            }

            return base.MultedBy(other);
        }

        public override Tuple<Error, Value> GetComparisonEQ(Value other)
        {
            if (other.GetType() == typeof(StringValue))
            {
                StringValue stringValue = (StringValue) other;
                return new Tuple<Error, Value>(default,
                    Value.Value == stringValue.Value.Value
                        ? NumberValue.True.Copy().SetContext(Context)
                        : NumberValue.False.Copy().SetContext(Context));
            }

            return base.GetComparisonEQ(other);
        }

        public override Tuple<Error, Value> GetComparisonNEQ(Value other)
        {
            if (other.GetType() == typeof(StringValue))
            {
                StringValue stringValue = (StringValue) other;
                return new Tuple<Error, Value>(default,
                    Value.Value != stringValue.Value.Value
                        ? NumberValue.True.Copy().SetContext(Context)
                        : NumberValue.False.Copy().SetContext(Context));
            }

            return base.GetComparisonNEQ(other);
        }

        public override Value Copy()
        {
            StringValue copy = new StringValue(Value);
            return copy.SetContext(Context).SetPosition(StartPos, EndPos);
        }

        public override string __repr__(int stage)
        {
            return $"\"{Value.Value}\"";
        }

        public override string ToString()
        {
            return Value.Value;
        }
    }
}
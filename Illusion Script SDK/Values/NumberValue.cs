using System;
using IllusionScript.SDK.Errors;

namespace IllusionScript.SDK.Values
{
    public class NumberValue : Value
    {
        public TokenValue Value;

        public NumberValue(TokenValue value)
        {
            Value = value;
        }

        public override Tuple<Error, Value> AddedTo(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                var otherNum = otherNumber.Value.IsInt()
                    ? otherNumber.Value.GetAsInt()
                    : otherNumber.Value.GetAsFloat();
                var currentNum = Value.IsInt() ? Value.GetAsInt() : Value.GetAsFloat();
                var res = currentNum + otherNum;
                TokenValue value;
                string resStr = res.ToString();
                if (resStr.Contains("."))
                {
                    value = new TokenValue(typeof(float), resStr);
                }
                else
                {
                    value = new TokenValue(typeof(int), resStr);
                }

                return new Tuple<Error, Value>(default, new NumberValue(value).SetContext(Context));
            }

            return base.AddedTo(other);
        }

        public override Tuple<Error, Value> SubbedBy(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                var otherNum = otherNumber.Value.IsInt()
                    ? otherNumber.Value.GetAsInt()
                    : otherNumber.Value.GetAsFloat();
                var currentNum = Value.IsInt() ? Value.GetAsInt() : Value.GetAsFloat();
                var res = currentNum - otherNum;
                TokenValue value;
                string resStr = res.ToString();
                if (resStr.Contains("."))
                {
                    value = new TokenValue(typeof(float), resStr);
                }
                else
                {
                    value = new TokenValue(typeof(int), resStr);
                }

                return new Tuple<Error, Value>(default, new NumberValue(value).SetContext(Context));
            }

            return base.SubbedBy(other);
        }

        public override Tuple<Error, Value> MultedBy(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                float res = Value.GetAsFloat() * otherNumber.Value.GetAsFloat();
                TokenValue value;
                string resStr = res.ToString();
                if (resStr.Contains("."))
                {
                    value = new TokenValue(typeof(float), resStr);
                }
                else
                {
                    value = new TokenValue(typeof(int), resStr);
                }

                return new Tuple<Error, Value>(default, new NumberValue(value).SetContext(Context));
            }

            return base.MultedBy(other);
        }

        public override Tuple<Error, Value> DivedBy(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;

                if (otherNumber.Value.Value == "0")
                {
                    return new Tuple<Error, Value>(new RuntimeError("Division by zero", Context, other.StartPos,
                        other.EndPos), default);
                }

                float res = Value.GetAsFloat() / otherNumber.Value.GetAsFloat();
                TokenValue value;
                string resStr = res.ToString();
                if (resStr.Contains("."))
                {
                    value = new TokenValue(typeof(float), resStr);
                }
                else
                {
                    value = new TokenValue(typeof(int), resStr);
                }

                return new Tuple<Error, Value>(default, new NumberValue(value).SetContext(Context));
            }

            return base.DivedBy(other);
        }

        public override Tuple<Error, Value> ModuloedBy(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                float res = Value.GetAsFloat() % otherNumber.Value.GetAsFloat();
                TokenValue value;
                string resStr = res.ToString();
                if (resStr.Contains("."))
                {
                    value = new TokenValue(typeof(float), resStr);
                }
                else
                {
                    value = new TokenValue(typeof(int), resStr);
                }

                return new Tuple<Error, Value>(default, new NumberValue(value).SetContext(Context));
            }

            return base.ModuloedBy(other);
        }

        public override Tuple<Error, Value> PowedBy(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                double res = Math.Pow(Value.GetAsFloat(), otherNumber.Value.GetAsFloat());
                TokenValue value;
                string resStr = res.ToString();
                if (resStr.Contains("."))
                {
                    value = new TokenValue(typeof(float), resStr);
                }
                else
                {
                    value = new TokenValue(typeof(int), resStr);
                }

                return new Tuple<Error, Value>(default, new NumberValue(value).SetContext(Context));
            }

            return base.SubbedBy(other);
        }

        public override Tuple<Error, Value> GetComparisonEQ(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                if (Value.Matches(otherNumber.Value))
                {
                    return new Tuple<Error, Value>(default, True.Copy().SetContext(Context));
                }
                else
                {
                    return new Tuple<Error, Value>(default, False.Copy().SetContext(Context));
                }
            }

            return base.GetComparisonEQ(other);
        }

        public override Tuple<Error, Value> GetComparisonNEQ(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                if (!Value.Matches(otherNumber.Value))
                {
                    return new Tuple<Error, Value>(default, True.Copy().SetContext(Context));
                }
                else
                {
                    return new Tuple<Error, Value>(default, False.Copy().SetContext(Context));
                }
            }

            return base.GetComparisonNEQ(other);
        }

        public override Tuple<Error, Value> GetComparisonLT(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                float num1 = Value.GetAsFloat();
                float num2 = otherNumber.Value.GetAsFloat();
                if (num1 < num2)
                {
                    return new Tuple<Error, Value>(default, True.Copy().SetContext(Context));
                }
                else
                {
                    return new Tuple<Error, Value>(default, False.Copy().SetContext(Context));
                }
            }

            return base.GetComparisonLT(other);
        }

        public override Tuple<Error, Value> GetComparisonGT(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                float num1 = Value.GetAsFloat();
                float num2 = otherNumber.Value.GetAsFloat();
                if (num1 > num2)
                {
                    return new Tuple<Error, Value>(default, True.Copy().SetContext(Context));
                }
                else
                {
                    return new Tuple<Error, Value>(default, False.Copy().SetContext(Context));
                }
            }

            return base.GetComparisonGT(other);
        }

        public override Tuple<Error, Value> GetComparisonLEQ(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                float num1 = Value.GetAsFloat();
                float num2 = otherNumber.Value.GetAsFloat();
                if (num1 <= num2)
                {
                    return new Tuple<Error, Value>(default, True.Copy().SetContext(Context));
                }
                else
                {
                    return new Tuple<Error, Value>(default, False.Copy().SetContext(Context));
                }
            }

            return base.GetComparisonLEQ(other);
        }

        public override Tuple<Error, Value> GetComparisonGEQ(Value other)
        {
            if (other.GetType() == typeof(NumberValue))
            {
                NumberValue otherNumber = (NumberValue) other;
                float num1 = Value.GetAsFloat();
                float num2 = otherNumber.Value.GetAsFloat();
                if (num1 >= num2)
                {
                    return new Tuple<Error, Value>(default, True.Copy().SetContext(Context));
                }
                else
                {
                    return new Tuple<Error, Value>(default, False.Copy().SetContext(Context));
                }
            }

            return base.GetComparisonGEQ(other);
        }

        public override Tuple<Error, Value> Notted()
        {
            return Value.GetAsFloat() == 1F
                ? new Tuple<Error, Value>(default, True.Copy().SetContext(Context))
                : new Tuple<Error, Value>(default, False.SetContext(Context));
        }

        public override bool IsTrue()
        {
            return Value.GetAsFloat() != 0;
        }

        public override Value Copy()
        {
            NumberValue copy = new NumberValue(Value);
            return copy.SetContext(Context).SetPosition(StartPos, EndPos);
        }

        public override string __repr__(int stage)
        {
            return Value.GetAsString();
        }

        public static readonly NumberValue True = new NumberValue(new TokenValue(typeof(int), "1"));
        public static readonly NumberValue False = new NumberValue(new TokenValue(typeof(int), "0"));
        public static NumberValue Null = new NumberValue(new TokenValue(typeof(int), "0"));
    }
}
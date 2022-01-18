using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class FieldValue : ClassItemValue
    {
        public FieldValue(string name, Value value) : base(name, value)
        {
        }

        public override Value Copy()
        {
            FieldValue value = new FieldValue(Name, Value);
            value.SetContext(Context);
            value.SetPosition(StartPos, EndPos);
            return value;
        }

        public override string __repr__(int stage)
        {
            return $"[{Name}:{Value.__repr__(stage)}]";
        }
    }
}
using IllusionScript.SDK.Nodes;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class FieldValue : ClassItemValue
    {
        public readonly Value Value;

        public FieldValue(Token contextIsolation, string name, Value value) : base(contextIsolation, name, default)
        {
            Value = value;
            Self = this;
        }

        public override Value Copy()
        {
            FieldValue value = new FieldValue(ContextIsolation, Name, Value);
            value.SetContext(Context);
            value.SetPosition(StartPos, EndPos);
            return value;
        }

        public override string __repr__(int stage)
        {
            return $"[{ContextIsolation.Value.GetAsString()}][{Name}:{Value.__repr__(stage)}]";
        }
    }
}
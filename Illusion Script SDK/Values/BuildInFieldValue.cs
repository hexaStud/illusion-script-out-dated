using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class BuildInFieldValue : ClassItemValue
    {
        public Value Value;
        public IBuildInField Field;

        public BuildInFieldValue(IBuildInField field) : base(field.Isolation, field.Name, default)
        {
            Self = this;
            Value = field.Value;
            Field = field;
        }

        public override Value Copy()
        {
            BuildInFieldValue copy = new BuildInFieldValue(Field);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override bool IsBuiltIn()
        {
            return true;
        }

        public override string __repr__(int stage)
        {
            return $"[{ContextIsolation.Value.GetAsString()}][{Name}:{Value.__repr__(stage)}]";
        }
    }
}
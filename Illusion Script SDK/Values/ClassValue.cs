using System.Collections.Generic;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class ClassValue : BaseClassValue
    {
        public List<ClassItemValue> Fields;
        public List<ClassItemValue> StaticFields;
        public ObjectValue StaticObject;
        public ClassValue Extends;

        public ClassValue(string name, List<ClassItemValue> fields, List<ClassItemValue> staticFields,
            ClassValue extends = default) : base(name)
        {
            Fields = fields;
            StaticFields = staticFields;
            Extends = extends;

            Dictionary<string, Value> staticObj = BuildStaticFields(staticFields);
            StaticObject = new ObjectValue(staticObj);
            StaticObject.SetContext(Context).SetPosition(StartPos, EndPos);
        }

        public override RuntimeResult Construct(List<Value> args)
        {
            RuntimeResult res = new RuntimeResult();
            Dictionary<string, Value> objData = ConvertFields(Fields);
            Context context = new Context($"<Class {Name}>", Context, StartPos);
            context.SymbolTable = new SymbolTable(Context.SymbolTable);

            objData["constructor"] =
                new ObjectValue(CreateConstructor(this, args)).SetContext(context).SetPosition(StartPos, EndPos);

            ObjectValue classObj = new ObjectValue(objData);
            classObj.SetContext(context).SetPosition(StartPos, EndPos);

            return res.Success(classObj);
        }

        public override Value Copy()
        {
            ClassValue value = new ClassValue(Name, Fields, StaticFields);
            value.SetContext(Context);
            value.SetPosition(StartPos, EndPos);
            return value;
        }

        public override string __repr__(int stage)
        {
            return $"<class {Name}[{StartPos.FileName} ({StartPos.Ln + 1})]>";
        }
    }
}
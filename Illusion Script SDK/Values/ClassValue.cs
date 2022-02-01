using System.Collections.Generic;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class ClassValue : BaseClassValue
    {
        public readonly ObjectValue StaticObject;

        public ClassValue(string name, List<ClassItemValue> fields, List<ClassItemValue> staticFields,
            BaseClassValue extends = default) : base(name, fields, staticFields, extends)
        {
            var staticObj = BuildStaticFields(staticFields);
            StaticObject = new ObjectValue(staticObj);
            StaticObject.SetContext(Context).SetPosition(StartPos, EndPos);
        }

        public override RuntimeResult Construct(List<Value> args)
        {
            ConstructorArgs = args;
            var res = new RuntimeResult();
            var objData = ConvertFields(Fields);
            var context = new Context($"<class {Name}>", Context, StartPos);
            context.SymbolTable = new SymbolTable(Context.SymbolTable);

            objData["constructor"] =
                new ObjectValue(CreateConstructor(this)).SetContext(context).SetPosition(StartPos, EndPos);

            var classObj = new ObjectValue(objData);
            classObj.SetContext(context).SetPosition(StartPos, EndPos);

            return res.Success(classObj);
        }

        public override Value Copy()
        {
            Value value = new ClassValue(Name, Fields, StaticFields, Extends);
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
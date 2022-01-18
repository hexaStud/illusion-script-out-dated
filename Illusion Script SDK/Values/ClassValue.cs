using System.Collections.Generic;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class ClassValue : BaseClassValue
    {
        public List<ClassItemValue> Fields;

        public ClassValue(string name, List<ClassItemValue> fields) : base(name)
        {
            Fields = fields;
        }

        public override RuntimeResult Construct(List<Value> args)
        {
            RuntimeResult res = new RuntimeResult();
            Dictionary<string, Value> objData = ConvertFields(Fields);
            Context context = new Context($"<Class {Name}>", Context, StartPos);
            context.SymbolTable = new SymbolTable(Context.SymbolTable);

            objData["constructor"] =
                new ObjectValue(CreateConstructor(this)).SetContext(context).SetPosition(StartPos, EndPos);

            ObjectValue classObj = new ObjectValue(objData);
            classObj.SetContext(context).SetPosition(StartPos, EndPos);

            context.SymbolTable.Set(Constants.Keyword.THIS, classObj, true);

            return res.Success(classObj);
        }

        public override Value Copy()
        {
            ClassValue value = new ClassValue(Name, Fields);
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
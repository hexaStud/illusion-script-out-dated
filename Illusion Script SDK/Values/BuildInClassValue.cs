using System.Collections.Generic;
using System.Data;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class BuildInClassValue : BaseClassValue
    {
        private IBuildInClass Template;

        private BuildInClassValue(string name, IBuildInClass buildInClass) : base(name, new List<ClassItemValue>(),
            new List<ClassItemValue>())
        {
            Template = buildInClass;
            List<ClassItemValue> fields = new List<ClassItemValue>();
            foreach (IBuildInMethod builtInMethod in buildInClass.Methods)
            {
                BuildInMethodValue methodValue = new BuildInMethodValue(builtInMethod);
                fields.Add(methodValue);
            }

            foreach (IBuildInField buildInField in buildInClass.Fields)
            {
                BuildInFieldValue fieldValue = new BuildInFieldValue(buildInField);
                fields.Add(fieldValue);
            }

            Fields = fields;

            List<ClassItemValue> staticFields = new List<ClassItemValue>();
            foreach (IBuildInMethod builtInMethod in buildInClass.StaticMethods)
            {
                BuildInMethodValue methodValue = new BuildInMethodValue(builtInMethod);
                staticFields.Add(methodValue);
            }

            foreach (IBuildInField buildInField in buildInClass.StaticFields)
            {
                BuildInFieldValue fieldValue = new BuildInFieldValue(buildInField);
                staticFields.Add(fieldValue);
            }

            StaticFields = staticFields;
        }

        public override RuntimeResult Construct(List<Value> args)
        {
            SetContext();
            ConstructorArgs = args;
            RuntimeResult res = new RuntimeResult();
            Dictionary<string, Value> objData = ConvertFields(Fields);
            Context context = new Context($"<class {Name}>", Context, StartPos);
            context.SymbolTable = new SymbolTable(Context.SymbolTable);

            objData["constructor"] = new ObjectValue(CreateConstructor(this)).SetContext(Context)
                .SetPosition(StartPos, EndPos);
            ObjectValue classValue = new ObjectValue(objData);
            classValue.SetContext(context).SetPosition(StartPos, EndPos);

            return res.Success(classValue);
        }

        public override Value Copy()
        {
            BuildInClassValue copy = new BuildInClassValue(Name, Template);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override string __repr__(int stage)
        {
            return $"<class {Name}[Native]>";
        }

        public override bool IsBuiltIn()
        {
            return true;
        }

        private void SetContext()
        {
            foreach (ClassItemValue itemValue in Fields)
            {
                itemValue.SetContext(Context);
            }

            foreach (ClassItemValue itemValue in StaticFields)
            {
                itemValue.SetContext(Context);
            }
        }

        public static BuildInClassValue Define(string name, IBuildInClass buildInClass)
        {
            return new BuildInClassValue(name, buildInClass);
        }
    }
}
using System.Collections.Generic;

namespace IllusionScript.SDK.Values.Assets
{
    public class BaseClassValue : Value
    {
        public MethodValue Constructor;
        public List<Value> ConstructorArgs;

        protected BaseClassValue Extends;
        protected List<ClassItemValue> Fields;
        public string Name;
        protected List<ClassItemValue> StaticFields;

        protected BaseClassValue(string name, List<ClassItemValue> fields, List<ClassItemValue> staticFields,
            BaseClassValue extends)
        {
            ConstructorArgs = new List<Value>();
            Constructor = default;
            Fields = fields;
            StaticFields = staticFields;
            Name = name;
            Extends = extends;
        }

        protected Dictionary<string, Value> ConvertFields(List<ClassItemValue> fields)
        {
            var convertedFields = new Dictionary<string, Value>();

            if (Extends != default(ClassValue))
            {
                var extends = Extends.ConvertFields(Extends.Fields);
                foreach (var extend in extends) convertedFields[extend.Key] = extend.Value;
            }

            foreach (var field in fields)
            {
                if (field.Name == Name && field.GetType() == typeof(MethodValue)) Constructor = (MethodValue)field;

                convertedFields[field.Name] = field.Self;
            }

            return convertedFields;
        }

        protected Dictionary<string, Value> BuildStaticFields(List<ClassItemValue> fields)
        {
            var convertedFields = new Dictionary<string, Value>();

            if (Extends != default(ClassValue))
            {
                var extends = Extends.BuildStaticFields(Extends.StaticFields);
                foreach (var extend in extends) convertedFields[extend.Key] = extend.Value;
            }

            foreach (var field in fields) convertedFields[field.Name] = field.Self;

            return convertedFields;
        }

        protected Dictionary<string, Value> CreateConstructor(BaseClassValue self)
        {
            var constructor = new Dictionary<string, Value>();
            constructor["className"] =
                new StringValue(new TokenValue(typeof(string), Name))
                    .SetContext(Context)
                    .SetPosition(StartPos, EndPos);

            constructor["class"] = self.Copy()
                .SetContext(Context)
                .SetPosition(StartPos, EndPos);

            if (self.Extends != default(ClassValue))
                constructor["extends"] = new ObjectValue(self.Extends.CreateConstructor(self.Extends))
                    .SetContext(Context).SetPosition(StartPos, EndPos);

            return constructor;
        }
    }
}
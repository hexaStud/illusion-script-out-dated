using System.Collections.Generic;

namespace IllusionScript.SDK.Values.Assets
{
    public class BaseClassValue : Value
    {
        public string Name;
        public List<Value> ConstructorArgs;
        public ClassItemValue Constructor;

        protected BaseClassValue Extends;
        protected List<ClassItemValue> Fields;
        public List<ClassItemValue> StaticFields;
        public ObjectValue StaticObject;

        protected BaseClassValue(string name, List<ClassItemValue> fields, List<ClassItemValue> staticFields,
            BaseClassValue extends = default)
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
            Dictionary<string, Value> convertedFields = new Dictionary<string, Value>();

            if (Extends != default(ClassValue))
            {
                Dictionary<string, Value> extends = Extends.ConvertFields(Extends.Fields);
                foreach (KeyValuePair<string, Value> extend in extends)
                {
                    convertedFields[extend.Key] = extend.Value;
                }
            }

            foreach (ClassItemValue field in fields)
            {
                if (field.Name == Name && field.GetType() == typeof(MethodValue) || 
                    field.Name == Name && field.GetType() == typeof(BuildInMethodValue))
                {
                    Constructor = field;
                }

                convertedFields[field.Name] = field.Self;
            }

            return convertedFields;
        }

        protected Dictionary<string, Value> BuildStaticFields(List<ClassItemValue> fields)
        {
            Dictionary<string, Value> convertedFields = new Dictionary<string, Value>();

            if (Extends != default(ClassValue))
            {
                Dictionary<string, Value> extends = Extends.BuildStaticFields(Extends.StaticFields);
                foreach (KeyValuePair<string, Value> extend in extends)
                {
                    convertedFields[extend.Key] = extend.Value;
                }
            }

            foreach (ClassItemValue field in fields)
            {
                convertedFields[field.Name] = field.Self;
            }

            return convertedFields;
        }

        protected Dictionary<string, Value> CreateConstructor(BaseClassValue self)
        {
            Dictionary<string, Value> constructor = new Dictionary<string, Value>();
            constructor["className"] =
                new StringValue(new TokenValue(typeof(string), Name))
                    .SetContext(Context)
                    .SetPosition(StartPos, EndPos);

            constructor["class"] = self.Copy()
                .SetContext(Context)
                .SetPosition(StartPos, EndPos);

            if (self.Extends != default(ClassValue))
            {
                constructor["extends"] = new ObjectValue(self.Extends.CreateConstructor(self.Extends))
                    .SetContext(Context).SetPosition(StartPos, EndPos);
            }

            return constructor;
        }
    }
}
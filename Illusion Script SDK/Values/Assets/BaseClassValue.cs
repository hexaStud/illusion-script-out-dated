﻿using System.Collections.Generic;
using IllusionScript.SDK.Nodes;

namespace IllusionScript.SDK.Values.Assets
{
    public class BaseClassValue : Value
    {
        protected string Name;

        protected BaseClassValue(string name)
        {
            Name = name;
        }

        protected Dictionary<string, Value> ConvertFields(List<ClassItemValue> fields)
        {
            Dictionary<string, Value> convertedFields = new Dictionary<string, Value>();

            foreach (ClassItemValue field in fields)
            {
                convertedFields[field.Name] = field.Self;
            }

            return convertedFields;
        }

        protected Dictionary<string, Value> BuildStaticFields(List<ClassItemValue> fields)
        {
            Dictionary<string, Value> convertedFields = new Dictionary<string, Value>();

            foreach (ClassItemValue field in fields)
            {
                convertedFields[field.Name] = field.Self;
            }

            return convertedFields;
        }

        protected Dictionary<string, Value> CreateConstructor(ClassValue self, List<Value> args)
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
                constructor["extends"] = self.Extends.Construct(args).Value;
            }

            return constructor;
        }
    }
}
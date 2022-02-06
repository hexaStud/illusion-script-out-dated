using System.Collections.Generic;
using IllusionScript.SDK.Extensions;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class ObjectValue : Value
    {
        public Dictionary<string, Value> Elements;

        public ObjectValue(Dictionary<string, Value> elements)
        {
            Elements = elements;
        }

        public override Value Copy()
        {
            ObjectValue copy = new ObjectValue(Elements);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override ObjectValue ObjectAccess()
        {
            ObjectValue objectValue = (ObjectValue)Copy();
            objectValue.Elements["get"] = BuildInFunctionValue.Define(GetFunc.Name, new GetFunc()).SetContext(Context)
                .SetPosition(StartPos, EndPos);
            return this;
        }

        public override string __repr__(int stage)
        {
            return "{" + Join(stage + 1);
        }

        private string Join(int stage)
        {
            string str = "\n" + Constants.TAB.Repeat(stage);
            bool first = true;

            foreach (KeyValuePair<string, Value> keyValuePair in Elements)
            {
                if (!first)
                {
                    str += ",\n" + Constants.TAB.Repeat(stage);
                }

                str += keyValuePair.Key + " = " + keyValuePair.Value.__repr__(stage + 1);

                first = false;
            }

            str += "\n" + Constants.TAB.Repeat(stage - 2) + "}";

            return str;
        }

        private class GetFunc : IBuildInFunction
        {
            public static string Name = "get";

            public List<string> Args { get; } = new List<string>()
            {
                "var"
            };

            public RuntimeResult Exec(Context context, BuildInFunctionValue self)
            {
                Value access = context.SymbolTable.Get("var").Value;
                ObjectValue objectValue = (ObjectValue)context.SymbolTable.Get("this").Value;
                if (access.GetType() == typeof(StringValue))
                {
                    string stringValue = ((StringValue)access).Value.GetAsString();

                    if (objectValue.Elements.ContainsKey(stringValue))
                    {
                        return new RuntimeResult().Success(objectValue.Elements[stringValue]);
                    }
                }

                return new RuntimeResult().Success(NumberValue.Null);
            }
        }
    }
}
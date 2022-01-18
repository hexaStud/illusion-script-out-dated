using System.Collections.Generic;
using IllusionScript.SDK.Extensions;

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
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using IllusionScript.SDK.Extensions;

namespace IllusionScript.SDK.Values
{
    public class ListValue : Value
    {
        public List<Value> Elements;

        public ListValue(List<Value> elements)
        {
            Elements = elements;
        }

        public override Value Copy()
        {
            ListValue copy = new ListValue(Elements);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override string __repr__(int stage)
        {
            return $"[{Join(stage + 1)}";
        }

        private string Join(int stage)
        {
            string str = "\n" + Constants.TAB.Repeat(stage);
            bool first = true;
            foreach (Value element in Elements)
            {
                if (!first)
                {
                    str += ",\n" + Constants.TAB.Repeat(stage);
                }

                str += element.__repr__(stage);
                first = false;
            }

            str += "\n" + Constants.TAB.Repeat(stage - 2) + "]";

            return str;
        }
    }
}
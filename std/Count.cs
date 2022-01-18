using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Errors;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;

namespace IllusionScript.Lib.std
{
    public class Count : IPlugin
    {
        public static string Name = "count";
        public List<string> Args { get; } = new List<string>() {"ele"};

        public RuntimeResult Exec(Context context, BuiltInFunctionValue self)
        {
            Value ele = context.SymbolTable.Get("ele").Value;
            int count = 0;
            if (ele.GetType() == typeof(StringValue))
            {
                StringValue stringValue = (StringValue) ele;
                count = stringValue.Value.GetAsString().Length;
            }
            else if (ele.GetType() == typeof(ListValue))
            {
                ListValue listValue = (ListValue) ele;
                count = listValue.Elements.Count;
            }
            else
            {
                return new RuntimeResult().Failure(new RuntimeError(
                    "Argument must be of type string or list", context, self.StartPos, self.EndPos));
            }

            return new RuntimeResult().Success(new NumberValue(new TokenValue(typeof(int), count.ToString())));
        }
    }
}
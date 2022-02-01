using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.std
{
    public class ToString : IBuiltInFunction
    {
        public static string Name = "toString";

        public List<string> Args { get; } = new()
        {
            "ele"
        };

        public RuntimeResult Exec(Context context, BuiltInFunctionValue self)
        {
            var ele = context.SymbolTable.Get("ele").Value;
            if (ele.GetType() == typeof(StringValue))
                return new RuntimeResult().Success(ele);
            return new RuntimeResult().Success(new StringValue(new TokenValue(typeof(string), ele.__repr__(0))));
        }
    }
}
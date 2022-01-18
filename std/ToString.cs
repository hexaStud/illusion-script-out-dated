using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;

namespace IllusionScript.Lib.std
{
    public class ToString : IPlugin
    {
        public static string Name = "toString";
        
        public List<string> Args { get; } = new List<string>()
        {
            "ele"
        };

        public RuntimeResult Exec(Context context, BuiltInFunctionValue self)
        {
            Value ele = context.SymbolTable.Get("ele").Value;
            if (ele.GetType() == typeof(StringValue))
            {
                return new RuntimeResult().Success(ele);
            }
            else
            {
                return new RuntimeResult().Success(new StringValue(new TokenValue(typeof(string), ele.__repr__(0))));
            }
        }
    }
}
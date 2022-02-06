using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.system.std
{
    public class ToString : IBuildInFunction
    {
        public static string Name = "toString";
        
        public List<string> Args { get; } = new List<string>()
        {
            "ele"
        };

        public RuntimeResult Exec(Context context, BuildInFunctionValue self)
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
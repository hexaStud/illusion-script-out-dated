using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.system.std
{
    public class IsList : IBuildInFunction
    {
        public static string Name = "isList";
        
        public List<string> Args { get; } = new List<string>()
        {
            "ele"
        };

        public RuntimeResult Exec(Context context, BuildInFunctionValue self)
        {
            Value ele = context.SymbolTable.Get("ele").Value;

            return new RuntimeResult().Success(ele.GetType() == typeof(ListValue)
                ? NumberValue.True
                : NumberValue.False);
        }
    }
}
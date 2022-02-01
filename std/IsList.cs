using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.std
{
    public class IsList : IBuiltInFunction
    {
        public static string Name = "isList";

        public List<string> Args { get; } = new()
        {
            "ele"
        };

        public RuntimeResult Exec(Context context, BuiltInFunctionValue self)
        {
            var ele = context.SymbolTable.Get("ele").Value;

            return new RuntimeResult().Success(ele.GetType() == typeof(ListValue)
                ? NumberValue.True
                : NumberValue.False);
        }
    }
}
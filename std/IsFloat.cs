using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.std
{
    public class IsFloat : IBuiltInFunction
    {
        public static string Name = "isFloat";

        public List<string> Args { get; } = new()
        {
            "ele"
        };

        public RuntimeResult Exec(Context context, BuiltInFunctionValue self)
        {
            var ele = context.SymbolTable.Get("ele").Value;

            return new RuntimeResult().Success(
                ele.GetType() == typeof(NumberValue) && ele.__repr__(0).IndexOf('.') != -1
                    ? NumberValue.True
                    : NumberValue.False);
        }
    }
}
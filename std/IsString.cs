using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.std
{
    public class IsString : IBuildInFunction
    {
        public static string Name = "isString";
        
        public List<string> Args { get; } = new List<string>()
        {
            "ele"
        };
        public RuntimeResult Exec(Context context, BuiltInFunctionValue self)
        {
            Value ele = context.SymbolTable.Get("ele").Value;

            return new RuntimeResult().Success(ele.GetType() == typeof(StringValue)
                ? NumberValue.True
                : NumberValue.False);
        }
    }
}
using System;
using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.std
{
    public class Print : IBuildInFunction
    {
        public static string Name = "print";

        public List<string> Args { get; } = new List<string>()
        {
            "x"
        };

        public RuntimeResult Exec(Context context, BuildInFunctionValue self)
        {
            Value value = context.SymbolTable.Get("x").Value;
            if (value.GetType() == typeof(StringValue))
            {
                StringValue str = (StringValue) value;
                Console.Write(str.ToString());
            }
            else
            {
                Console.Write(value.__repr__(0));
            }

            return new RuntimeResult().Success(NumberValue.Null);
        }
    }
}
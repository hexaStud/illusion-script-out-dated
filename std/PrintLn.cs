using System;
using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.std
{
    public class PrintLn : IBuildInFunction
    {
        public static string Name = "println";
        public List<string> Args { get; } = new List<string>() { "x" };

        public RuntimeResult Exec(Context context, BuiltInFunctionValue self)
        {
            Value x = context.SymbolTable.Get("x").Value;

            if (x.GetType() == typeof(StringValue))
            {
                StringValue str = (StringValue)x;
                Console.Write(str.ToString());
            }
            else
            {
                Console.Write(x.__repr__(0));
            }

            Console.Write(Constants.EOL);
            return new RuntimeResult().Success(NumberValue.Null);
        }
    }
}
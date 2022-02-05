using System;
using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.std
{
    public class Read : IBuildInFunction
    {
        public static string Name = "read";
        public List<string> Args { get; } = new List<string>();

        public RuntimeResult Exec(Context context, BuiltInFunctionValue self)
        {
            string a = Console.ReadLine();
            return new RuntimeResult().Success(new StringValue(new TokenValue(typeof(string), a)));
        }
    }
}
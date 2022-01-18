using System;
using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;

namespace IllusionScript.Lib.std
{
    public class Read : IPlugin
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
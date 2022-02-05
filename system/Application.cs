using System;
using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.Lib.system
{
    public class Application : IBuildInClass
    {
        public static string ClassName = "Application";
        public string Name { get; } = ClassName;

        public List<IBuildInMethod> Methods { get; } = new List<IBuildInMethod>()
        {
            new Exit()
        };

        public List<IBuildInField> Fields { get; } = new List<IBuildInField>()
        {
            new Test()
        };

        public List<IBuildInMethod> StaticMethods { get; } = new List<IBuildInMethod>() { };
        public List<IBuildInField> StaticFields { get; } = new List<IBuildInField>() { };

        private class Exit : IBuildInMethod
        {
            public string Name { get; } = "Application";
            public Token Isolation { get; } = IBuildInMethod.PUBLIC;
            public List<string> ArgNames { get; } = new List<string>();

            public RuntimeResult Exec(List<Value> args, BuildInMethodValue self)
            {
                Console.WriteLine("Native constructor exec");
                return new RuntimeResult().Success(NumberValue.Null);
            }
        }

        private class Test : IBuildInField
        {
            public string Name { get; } = "Test";
            public Token Isolation { get; } = IBuildInClassItem.PUBLIC;
            public Value Value { get; set; } = new StringValue(new TokenValue(typeof(string), "Hello"));
        }
    }
}
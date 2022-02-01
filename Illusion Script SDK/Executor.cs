using System;
using System.Collections.Generic;
using IllusionScript.SDK.Nodes;

namespace IllusionScript.SDK
{
    public static class Executor
    {
        public static Tuple<Error, Value, Dictionary<string, Value>> Run(string text, string fileName,
            string filePath, Context context, bool main = false)
        {
            var lexer = new Lexer(text, fileName, filePath);
            var res = lexer.MakeTokens();

            if (res.Item1 != default(Error))
                return new Tuple<Error, Value, Dictionary<string, Value>>(res.Item1, default, default);

            var parser = new Parser(res.Item2);
            var parserResult = parser.Parse();

            if (parserResult.Error != default(Error))
                return new Tuple<Error, Value, Dictionary<string, Value>>(parserResult.Error, default, default);

            return RunAst((ListNode)parserResult.Node, context, main);
        }

        public static Tuple<Error, Value, Dictionary<string, Value>> RunAst(ListNode node, Context context,
            bool main)
        {
            if (main) node.Elements.Add(new MainNode());

            var interpreter = new Interpreter();

            var interpreterResult = interpreter.Visit(node, context);
            return new Tuple<Error, Value, Dictionary<string, Value>>(interpreterResult.Error,
                interpreterResult.Value, interpreter.Exports);
        }
    }
}
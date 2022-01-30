using System;
using System.Collections.Generic;
using IllusionScript.SDK.Nodes;

namespace IllusionScript.SDK
{
    public static class Executor
    {
        public static Tuple<Error, Value, Dictionary<string, Value>> Run(string text, string fileName,
            string filePath, Context context,
            bool main = false)
        {
            Lexer lexer = new Lexer(text, fileName, filePath);
            Tuple<Error, List<Token>> res = lexer.MakeTokens();

            if (res.Item1 != default(Error))
            {
                return new Tuple<Error, Value, Dictionary<string, Value>>(res.Item1, default, default);
            }

            Parser parser = new Parser(res.Item2);
            ParserResult parserResult = parser.Parse();

            if (parserResult.Error != default(Error))
            {
                return new Tuple<Error, Value, Dictionary<string, Value>>(parserResult.Error, default, default);
            }

            ListNode node = (ListNode) parserResult.Node;
            if (main)
            {
                node.Elements.Add(new MainNode());
            }

            Interpreter interpreter = new Interpreter();
            RuntimeResult interpreterResult = interpreter.Visit(node, context);
            return new Tuple<Error, Value, Dictionary<string, Value>>(interpreterResult.Error,
                interpreterResult.Value, interpreter.Exports);
        }
    }
}
using System;
using System.IO;
using IllusionScript.SDK;
using NUnit.Framework;

namespace IllusionScript.Tests.LexerTest
{
    public class If
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IfTest()
        {
            var lexer = new Lexer("if 5 == 2 then; 5; end;", "<test>", Directory.GetCurrentDirectory());
            var lexerResult = lexer.MakeTokens();
            if (lexerResult.Item1 != default(Error))
            {
                Console.WriteLine(lexerResult.Item1.ToString());
                Assert.Fail();
                return;
            }

            var tokens = lexerResult.Item2;

            foreach (var token in tokens) Console.WriteLine(token.__repr__());

            if (tokens[0].Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), "if")) &&
                tokens[1].Matches(Constants.TT.INT, new TokenValue(typeof(int), "5")) &&
                tokens[2].Type == Constants.TT.DOUBLE_EQUALS &&
                tokens[3].Matches(Constants.TT.INT, new TokenValue(typeof(int), "2")) &&
                tokens[4].Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), "then")) &&
                tokens[5].Type == Constants.TT.NEWLINE &&
                tokens[6].Matches(Constants.TT.INT, new TokenValue(typeof(int), "5")) &&
                tokens[7].Type == Constants.TT.NEWLINE &&
                tokens[8].Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), "end")) &&
                tokens[9].Type == Constants.TT.NEWLINE &&
                tokens[10].Type == Constants.TT.EOF
               )
                return;

            Assert.Fail();
        }
    }
}
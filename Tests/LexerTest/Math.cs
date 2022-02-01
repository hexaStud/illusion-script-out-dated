using System;
using System.IO;
using IllusionScript.SDK;
using NUnit.Framework;

namespace IllusionScript.Tests.LexerTest
{
    public class Math
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PlusTest()
        {
            var lexer = new Lexer("1 + 2", "<test>", Directory.GetCurrentDirectory());
            var lexerResult = lexer.MakeTokens();
            if (lexerResult.Item1 != default(Error))
            {
                Console.WriteLine(lexerResult.Item1.ToString());
                Assert.Fail();
                return;
            }

            var tokens = lexerResult.Item2;
            foreach (var token in tokens) Console.WriteLine(token.__repr__());
            if (tokens[0].Matches(Constants.TT.INT, new TokenValue(typeof(int), "1")) &&
                tokens[1].Type == Constants.TT.PLUS &&
                tokens[2].Matches(Constants.TT.INT, new TokenValue(typeof(int), "2")) &&
                tokens[3].Type == Constants.TT.EOF
               )
                return;

            Assert.Fail();
        }

        [Test]
        public void MinusTest()
        {
            var lexer = new Lexer("3 - 4", "<test>", Directory.GetCurrentDirectory());
            var lexerResult = lexer.MakeTokens();
            if (lexerResult.Item1 != default(Error))
            {
                Console.WriteLine(lexerResult.Item1.ToString());
                Assert.Fail();
                return;
            }

            var tokens = lexerResult.Item2;
            foreach (var token in tokens) Console.WriteLine(token.__repr__());
            if (tokens[0].Matches(Constants.TT.INT, new TokenValue(typeof(int), "3")) &&
                tokens[1].Type == Constants.TT.MINUS &&
                tokens[2].Matches(Constants.TT.INT, new TokenValue(typeof(int), "4")) &&
                tokens[3].Type == Constants.TT.EOF
               )
                return;

            Assert.Fail();
        }

        [Test]
        public void MultiplyTest()
        {
            var lexer = new Lexer("5 * 6", "<test>", Directory.GetCurrentDirectory());
            var lexerResult = lexer.MakeTokens();
            if (lexerResult.Item1 != default(Error))
            {
                Console.WriteLine(lexerResult.Item1.ToString());
                Assert.Fail();
                return;
            }

            var tokens = lexerResult.Item2;
            foreach (var token in tokens) Console.WriteLine(token.__repr__());
            if (tokens[0].Matches(Constants.TT.INT, new TokenValue(typeof(int), "5")) &&
                tokens[1].Type == Constants.TT.MUL &&
                tokens[2].Matches(Constants.TT.INT, new TokenValue(typeof(int), "6")) &&
                tokens[3].Type == Constants.TT.EOF
               )
                return;

            Assert.Fail();
        }

        [Test]
        public void DivideTest()
        {
            var lexer = new Lexer("7 / 8", "<test>", Directory.GetCurrentDirectory());
            var lexerResult = lexer.MakeTokens();
            if (lexerResult.Item1 != default(Error))
            {
                Console.WriteLine(lexerResult.Item1.ToString());
                Assert.Fail();
                return;
            }

            var tokens = lexerResult.Item2;
            foreach (var token in tokens) Console.WriteLine(token.__repr__());
            if (tokens[0].Matches(Constants.TT.INT, new TokenValue(typeof(int), "7")) &&
                tokens[1].Type == Constants.TT.DIV &&
                tokens[2].Matches(Constants.TT.INT, new TokenValue(typeof(int), "8")) &&
                tokens[3].Type == Constants.TT.EOF
               )
                return;

            Assert.Fail();
        }

        [Test]
        public void PowTest()
        {
            var lexer = new Lexer("9 ^ 1.5", "<test>", Directory.GetCurrentDirectory());
            var lexerResult = lexer.MakeTokens();
            if (lexerResult.Item1 != default(Error))
            {
                Console.WriteLine(lexerResult.Item1.ToString());
                Assert.Fail();
                return;
            }

            var tokens = lexerResult.Item2;
            foreach (var token in tokens) Console.WriteLine(token.__repr__());
            if (tokens[0].Matches(Constants.TT.INT, new TokenValue(typeof(int), "9")) &&
                tokens[1].Type == Constants.TT.POW &&
                tokens[2].Matches(Constants.TT.FLOAT, new TokenValue(typeof(float), "1.5")) &&
                tokens[3].Type == Constants.TT.EOF
               )
                return;

            Assert.Fail();
        }

        [Test]
        public void ComplexMathTest()
        {
            var lexer = new Lexer("(5 + 5) * 2", "<test>", Directory.GetCurrentDirectory());
            var lexerResult = lexer.MakeTokens();
            if (lexerResult.Item1 != default(Error))
            {
                Console.WriteLine(lexerResult.Item1.ToString());
                Assert.Fail();
                return;
            }

            var tokens = lexerResult.Item2;
            foreach (var token in tokens) Console.WriteLine(token.__repr__());
            if (tokens[0].Type == Constants.TT.LPAREN &&
                tokens[1].Matches(Constants.TT.INT, new TokenValue(typeof(int), "5")) &&
                tokens[2].Type == Constants.TT.PLUS &&
                tokens[3].Matches(Constants.TT.INT, new TokenValue(typeof(int), "5")) &&
                tokens[4].Type == Constants.TT.RPAREN &&
                tokens[5].Type == Constants.TT.MUL &&
                tokens[6].Matches(Constants.TT.INT, new TokenValue(typeof(int), "2")) &&
                tokens[7].Type == Constants.TT.EOF
               )
                return;

            Assert.Fail();
        }
    }
}
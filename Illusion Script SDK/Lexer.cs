using System;
using System.Collections.Generic;
using System.Linq;
using IllusionScript.SDK;
using IllusionScript.SDK.Errors;

namespace IllusionScript.SDK
{
    public class Lexer
    {
        private string Text;
        private Position Pos;
        private char CurrentChar;

        public Lexer(string text, string fileName, string filepath)
        {
            Text = text;
            Pos = new Position(-1, 0, -1, fileName, filepath, text);
            Advance();
        }

        private void Advance()
        {
            Pos.Advance(CurrentChar);
            if (Pos.Idx < Text.Length)
            {
                CurrentChar = Text[Pos.Idx];
            }
            else
            {
                CurrentChar = default;
            }
        }

        public Tuple<Error, List<Token>> MakeTokens()
        {
            List<Token> tokens = new List<Token>();

            while (CurrentChar != default(char))
            {
                if (Constants.IGNORE_CHARACTERS.Contains(CurrentChar))
                {
                    Advance();
                }
                else if (CurrentChar == '#')
                {
                    SkipComment();
                }
                else if (CurrentChar is ';' or '\n')
                {
                    tokens.Add(new Token(Constants.TT.NEWLINE, default, Pos));
                    Advance();
                }
                else if (Constants.DIGITS.Contains(CurrentChar))
                {
                    tokens.Add(MakeNumber());
                }
                else if (Constants.LETTERS.Contains(CurrentChar))
                {
                    tokens.Add(MakeIdentifier());
                }
                else if (CurrentChar == '"')
                {
                    tokens.Add(MakeString());
                }
                else if (CurrentChar == '+')
                {
                    tokens.Add(new Token(Constants.TT.PLUS, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '-')
                {
                    tokens.Add(new Token(Constants.TT.MINUS, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '*')
                {
                    tokens.Add(new Token(Constants.TT.MUL, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '/')
                {
                    tokens.Add(new Token(Constants.TT.DIV, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '%')
                {
                    tokens.Add(new Token(Constants.TT.MODULO, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '^')
                {
                    tokens.Add(new Token(Constants.TT.POW, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '(')
                {
                    tokens.Add(new Token(Constants.TT.LPAREN, default, Pos));
                    Advance();
                }
                else if (CurrentChar == ')')
                {
                    tokens.Add(new Token(Constants.TT.RPAREN, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '[')
                {
                    tokens.Add(new Token(Constants.TT.LBRACKET, default, Pos));
                    Advance();
                }
                else if (CurrentChar == ']')
                {
                    tokens.Add(new Token(Constants.TT.RBRACKET, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '{')
                {
                    tokens.Add(new Token(Constants.TT.LCURLY_BRACKET, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '}')
                {
                    tokens.Add(new Token(Constants.TT.RCURLY_BRACKET, default, Pos));
                    Advance();
                }
                else if (CurrentChar == ',')
                {
                    tokens.Add(new Token(Constants.TT.COMMA, default, Pos));
                    Advance();
                }
                else if (CurrentChar == '!')
                {
                    Tuple<Error, Token> res = MakeNotEquals();
                    if (res.Item1 == default(Error))
                    {
                        return new Tuple<Error, List<Token>>(res.Item1, default);
                    }

                    tokens.Add(res.Item2);
                }
                else if (CurrentChar == '=')
                {
                    tokens.Add(MakeEquals());
                }
                else if (CurrentChar == '<')
                {
                    tokens.Add(MakeLessThan());
                }
                else if (CurrentChar == '>')
                {
                    tokens.Add(MakeGreaterThan());
                }
                else if (CurrentChar == '@')
                {
                    Tuple<Error, Token> res = MakeHeader();
                    if (res.Item1 != default(Error))
                    {
                        return new Tuple<Error, List<Token>>(res.Item1, default);
                    }
                    else
                    {
                        tokens.Add(res.Item2);
                    }
                }
                else
                {
                    Position startPos = Pos.Copy();
                    char c = CurrentChar;
                    Advance();
                    return new Tuple<Error, List<Token>>(new IllegalCharError($"'{c}'", startPos, Pos), default);
                }
            }

            tokens.Add(new Token(Constants.TT.EOF, default, Pos));
            return new Tuple<Error, List<Token>>(default, tokens);
        }

        private Token MakeNumber()
        {
            string numStr = "";
            int dotCount = 0;
            Position startPos = Pos.Copy();

            while (CurrentChar != default(char) && Constants.DIGITS.Contains(CurrentChar) || CurrentChar == '.')
            {
                if (CurrentChar == '.')
                {
                    if (dotCount == 1)
                    {
                        break;
                    }

                    dotCount++;
                    numStr += CurrentChar;
                }
                else
                {
                    numStr += CurrentChar;
                }

                Advance();
            }

            if (dotCount == 0)
            {
                return new Token(Constants.TT.INT, new TokenValue(typeof(int), numStr), startPos, Pos);
            }
            else
            {
                return new Token(Constants.TT.FLOAT, new TokenValue(typeof(float), numStr), startPos, Pos);
            }
        }

        private Token MakeIdentifier()
        {
            string idStr = "";
            Position startPos = Pos.Copy();

            while (CurrentChar != default(char) && Constants.LETTERS_DIGITS.Contains(CurrentChar) || CurrentChar == '_')
            {
                idStr += CurrentChar;
                Advance();
            }

            string tokenType = (Constants.Keyword.KEYWORDS.Contains(idStr))
                ? Constants.TT.KEYWORD
                : Constants.TT.IDENTIFIER;

            return new Token(tokenType, new TokenValue(typeof(string), idStr), startPos, Pos);
        }

        private Tuple<Error, Token> MakeHeader()
        {
            string idStr = "";
            Position startPos = Pos.Copy();

            while (CurrentChar != default(char) && Constants.LETTERS_DIGITS.Contains(CurrentChar) ||
                   CurrentChar is '_' or '@')
            {
                idStr += CurrentChar;
                Advance();
            }

            if (Constants.HeadKeyword.KEYWORDS.Contains(idStr))
            {
                return new Tuple<Error, Token>(default,
                    new Token(Constants.TT.HEAD_KEYWORD, new TokenValue(typeof(string), idStr), startPos, Pos));
            }
            else
            {
                return new Tuple<Error, Token>(new UnexpectedCharError("Unexpected char '@'", startPos, Pos), default);
            }
        }

        private Tuple<Error, Token> MakeNotEquals()
        {
            Position startPos = Pos.Copy();
            Advance();

            if (CurrentChar == '=')
            {
                Advance();
                return new Tuple<Error, Token>(default, new Token(Constants.TT.NOT_EQUALS, default, startPos, Pos));
            }

            return new Tuple<Error, Token>(new ExpectedCharError("'=' (after '!')", startPos, Pos), default);
        }

        private Token MakeEquals()
        {
            Position startPos = Pos.Copy();
            string tokenType = Constants.TT.EQUALS;
            Advance();

            if (CurrentChar == '=')
            {
                Advance();
                tokenType = Constants.TT.DOUBLE_EQUALS;
            }
            else if (CurrentChar == '>')
            {
                Advance();
                tokenType = Constants.TT.ARROW;
            }

            return new Token(tokenType, default, startPos, Pos);
        }

        private Token MakeLessThan()
        {
            Position startPos = Pos.Copy();
            string tokenType = Constants.TT.LESS_THAN;
            Advance();

            if (CurrentChar == '=')
            {
                Advance();
                tokenType = Constants.TT.LESS_EQUALS;
            }

            return new Token(tokenType, default, startPos, Pos);
        }

        private Token MakeGreaterThan()
        {
            Position startPos = Pos.Copy();
            string tokenType = Constants.TT.GREATER_THAN;
            Advance();

            if (CurrentChar == '=')
            {
                Advance();
                tokenType = Constants.TT.GREATER_EQUALS;
            }
            else if (CurrentChar == '>')
            {
                Advance();
                tokenType = Constants.TT.ACCESS_ARROW;
            }

            return new Token(tokenType, default, startPos, Pos);
        }

        private Token MakeString()
        {
            Position startPos = Pos.Copy();
            string str = "";
            bool escapeChar = false;
            Advance();

            while (CurrentChar != default(char) && (CurrentChar != '"' || escapeChar))
            {
                if (escapeChar)
                {
                    if (Constants.ESCAPE_CHARACTERS.ContainsKey(CurrentChar))
                    {
                        str += Constants.ESCAPE_CHARACTERS.GetValueOrDefault(CurrentChar, CurrentChar);
                    }
                    else
                    {
                        str += CurrentChar;
                    }
                }
                else
                {
                    if (CurrentChar == '\\')
                    {
                        escapeChar = true;
                    }
                    else
                    {
                        str += CurrentChar;
                    }
                }

                Advance();
                escapeChar = false;
            }

            Advance();
            return new Token(Constants.TT.STRING, new TokenValue(typeof(string), str), startPos, Pos);
        }

        private void SkipComment()
        {
            Advance();

            while (CurrentChar != '\n')
            {
                Advance();
            }

            Advance();
        }
    }
}
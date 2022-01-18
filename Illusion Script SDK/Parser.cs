using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using IllusionScript.SDK.Nodes;
using IllusionScript.SDK.Nodes.Assets;
using IllusionScript.SDK.Errors;

namespace IllusionScript.SDK
{
    public class Parser
    {
        public List<Token> Tokens;
        public int TokenIdx;
        public Token CurrentToken;

        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
            TokenIdx = -1;
            Advance();
        }

        private Token Advance()
        {
            TokenIdx++;
            UpdateCurrentToken();
            return CurrentToken;
        }

        private Token Reverse(int index = 1)
        {
            TokenIdx -= index;
            UpdateCurrentToken();
            return CurrentToken;
        }

        private void UpdateCurrentToken()
        {
            if (TokenIdx >= 0 && TokenIdx < Tokens.Count)
            {
                CurrentToken = Tokens[TokenIdx];
            }
            else
            {
                CurrentToken = default;
            }
        }

        public ParserResult Parse()
        {
            ParserResult res = Statements();
            if (res.Error != default(Error) && CurrentToken.Type != Constants.TT.EOF)
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected '+', '-', '*', '/', '^', '==', '!=', '<', '>', <=', '>=', 'AND' or 'OR'",
                    CurrentToken.StartPos, CurrentToken.EndPos));
            }

            return res;
        }

        private ParserResult Statements()
        {
            ParserResult res = new ParserResult();
            List<Node> statements = new List<Node>();
            Position startPos = CurrentToken.StartPos;

            while (CurrentToken.Type == Constants.TT.NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();
            }

            Node statement = res.Register(Statement());
            if (res.Error != default(Error))
            {
                return res;
            }

            statements.Add(statement);
            bool moreStatements = true;

            while (true)
            {
                int newLineCount = 0;
                while (CurrentToken.Type == Constants.TT.NEWLINE)
                {
                    res.RegisterAdvancement();
                    Advance();
                    newLineCount++;
                }

                if (newLineCount == 0)
                {
                    moreStatements = false;
                }

                if (!moreStatements)
                {
                    break;
                }

                statement = res.TryRegister(Statement());
                if (statement == default(Node))
                {
                    Reverse(res.ToReverseCount);
                    moreStatements = false;
                    continue;
                }

                
                statements.Add(statement);
            }

            return res.Success(new ListNode(statements, startPos, CurrentToken.EndPos.Copy()));
        }

        private ParserResult Statement()
        {
            ParserResult res = new ParserResult();
            Position startPos = CurrentToken.StartPos.Copy();

            if (CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.RETURN)))
            {
                res.RegisterAdvancement();
                Advance();

                Node expr = res.TryRegister(Expr());
                if (expr != default(Node))
                {
                    Reverse(res.ToReverseCount);
                }

                return res.Success(new ReturnNode(expr, startPos, CurrentToken.StartPos.Copy()));
            }

            if (CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.CONTINUE)))
            {
                res.RegisterAdvancement();
                Advance();
                return res.Success(new ContinueNode(startPos, CurrentToken.StartPos.Copy()));
            }

            if (CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.BREAK)))
            {
                res.RegisterAdvancement();
                Advance();
                return res.Success(new BreakNode(startPos, CurrentToken.StartPos.Copy()));
            }

            Node expr2 = res.Register(Expr());
            if (expr2 == default(Node))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expect 'ret', 'con', 'break' 'let', 'for', 'while', 'define', int float, identifier, '+', '-','(', '[' or '!'",
                    startPos, CurrentToken.EndPos));
            }

            return res.Success(expr2);
        }

        private ParserResult Expr()
        {
            ParserResult res = new ParserResult();
            if (CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.VAR)) ||
                CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.CONST)))
            {
                string variableType = CurrentToken.Value.Value;
                res.RegisterAdvancement();
                Advance();

                if (CurrentToken.Type != Constants.TT.IDENTIFIER)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected identifier", CurrentToken.StartPos, CurrentToken.EndPos));
                }

                Token varName = CurrentToken;
                res.RegisterAdvancement();
                Advance();

                if (CurrentToken.Type != Constants.TT.EQUALS)
                {
                    return res.Failure(new InvalidSyntaxError("Expected '='", CurrentToken.StartPos,
                        CurrentToken.EndPos));
                }

                res.RegisterAdvancement();
                Advance();
                Node expr = res.Register(Expr());
                if (res.Error != default(Error))
                {
                    return res;
                }
                else
                {
                    if (variableType == Constants.Keyword.VAR)
                    {
                        return res.Success(new VarAssignNode(varName, expr, true));
                    }
                    else
                    {
                        return res.Success(new ConstAssignNode(varName, expr));
                    }
                }
            }

            Node node = res.Register(BinOp(CompExpr,
                new[]
                {
                    new Token(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.AND)),
                    new Token(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.OR))
                }));
            if (res.Error != default(Error))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expect 'var', 'for', 'while', 'define', int float, identifier, '+', '-','(', '[' or '!'",
                    CurrentToken.StartPos, CurrentToken.EndPos));
            }

            return res.Success(node);
        }

        private ParserResult CompExpr()
        {
            ParserResult res = new ParserResult();

            if (CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.NOT)))
            {
                Token opToken = CurrentToken;
                res.RegisterAdvancement();
                Advance();

                Node node = res.Register(CompExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(new UnaryOpNode(opToken, node));
            }

            Node node2 = res.Register(BinOp(ArithExpr,
                new[]
                {
                    Constants.TT.DOUBLE_EQUALS, Constants.TT.NOT_EQUALS, Constants.TT.LESS_THAN,
                    Constants.TT.LESS_EQUALS, Constants.TT.GREATER_THAN, Constants.TT.GREATER_EQUALS
                }));
            if (res.Error != default(Error))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected int, float, identifier, '+', '-', '(', '[', 'if', 'for', 'while', 'define' or '!'",
                    CurrentToken.StartPos,
                    CurrentToken.EndPos
                ));
            }

            return res.Success(node2);
        }


        private ParserResult ArithExpr()
        {
            return BinOp(Term, new[] {Constants.TT.PLUS, Constants.TT.MINUS});
        }

        private ParserResult Term()
        {
            return BinOp(Factor, new[] {Constants.TT.MUL, Constants.TT.DIV, Constants.TT.MODULO});
        }

        private ParserResult Factor()
        {
            ParserResult res = new ParserResult();
            Token tok = CurrentToken;

            if (CurrentToken.Type is Constants.TT.PLUS or Constants.TT.MINUS)
            {
                res.RegisterAdvancement();
                Advance();
                Node factor = res.Register(Factor());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(new UnaryOpNode(tok, factor));
            }

            return Power();
        }

        private ParserResult Power()
        {
            return BinOp(Call, new[] {Constants.TT.POW}, Factor);
        }

        private ParserResult Call()
        {
            ParserResult res = new ParserResult();
            Node atom = res.Register(Atom());
            if (res.Error != default(Error))
            {
                return res;
            }

            if (CurrentToken.Type == Constants.TT.LPAREN)
            {
                res.RegisterAdvancement();
                Advance();
                List<Node> argNodes = new List<Node>();
                if (CurrentToken.Type == Constants.TT.RPAREN)
                {
                    res.RegisterAdvancement();
                    Advance();
                }
                else
                {
                    argNodes.Add(res.Register(Expr()));
                    if (res.Error != default(Error))
                    {
                        return res.Failure(new InvalidSyntaxError(
                            "Expected ')', 'let', 'if', 'for', 'while', 'define', int, float, identifier, '+', '-', '(', '[' or '!'",
                            CurrentToken.StartPos, CurrentToken.EndPos));
                    }

                    while (CurrentToken.Type == Constants.TT.COMMA)
                    {
                        res.RegisterAdvancement();
                        Advance();

                        argNodes.Add(res.Register(Expr()));
                        if (res.Error != default(Error))
                        {
                            return res;
                        }
                    }

                    if (CurrentToken.Type != Constants.TT.RPAREN)
                    {
                        return res.Failure(new InvalidSyntaxError(
                            "Expected ')' or ','",
                            CurrentToken.StartPos, CurrentToken.EndPos));
                    }

                    res.RegisterAdvancement();
                    Advance();
                }

                if (atom.GetType() == typeof(ObjectAccessNode))
                {
                    return res.Success(new ObjectCallNode(atom, argNodes));
                }

                return res.Success(new CallNode(atom, argNodes));
            }

            return res.Success(atom);
        }

        private ParserResult Atom()
        {
            ParserResult res = new ParserResult();
            Token tok = CurrentToken;
            if (CurrentToken.Type is Constants.TT.INT or Constants.TT.FLOAT)
            {
                res.RegisterAdvancement();
                Advance();
                return res.Success(new NumberNode(tok));
            }
            else if (CurrentToken.Type == Constants.TT.STRING)
            {
                res.RegisterAdvancement();
                Advance();
                return res.Success(new StringNode(tok));
            }
            else if (CurrentToken.Type == Constants.TT.IDENTIFIER)
            {
                res.RegisterAdvancement();
                Advance();

                if (CurrentToken.Type == Constants.TT.ACCESS_ARROW)
                {
                    Node expr = res.Register(ObjExpr(tok));
                    if (res.Error != default(Error))
                    {
                        return res;
                    }

                    return res.Success(expr);
                }
                else if (CurrentToken.Type == Constants.TT.EQUALS)
                {
                    res.RegisterAdvancement();
                    Advance();

                    Node value = res.Register(Expr());
                    if (res.Error != default(Error))
                    {
                        return res;
                    }

                    return res.Success(new VarAssignNode(tok, value, false));
                }
                else
                {
                    return res.Success(new VarAccessNode(tok));
                }
            }
            else if (tok.Type == Constants.TT.LPAREN)
            {
                res.RegisterAdvancement();
                Advance();
                Node expr = res.Register(Expr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                if (CurrentToken.Type == Constants.TT.RPAREN)
                {
                    res.RegisterAdvancement();
                    Advance();
                    return res.Success(expr);
                }
                else
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected ')'", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }
            else if (tok.Type == Constants.TT.LBRACKET)
            {
                Node expr = res.Register(ListExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Type == Constants.TT.LCURLY_BRACKET)
            {
                Node expr = res.Register(CurlyExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.IF)))
            {
                Node expr = res.Register(IfExpr(false));
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.WHILE)))
            {
                Node expr = res.Register(WhileExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.FOR)))
            {
                Node expr = res.Register(ForExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.FUNCTION)))
            {
                Node expr = res.Register(FuncExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.CLASS)))
            {
                Node expr = res.Register(ClassExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.NEW)))
            {
                Node expr = res.Register(ClassConstructExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.EXPORT)))
            {
                Node expr = res.Register(ExportExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }
            else if (tok.Type == Constants.TT.HEAD_KEYWORD)
            {
                Node expr = res.Register(HeadExpr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(expr);
            }

            return res.Failure(new InvalidSyntaxError(
                "Expected int, float, identifier '+', '-', '(', '[', 'if', 'for', 'while', 'define'",
                CurrentToken.StartPos, CurrentToken.EndPos));
        }

        private ParserResult ObjExpr(Token tok)
        {
            ParserResult res = new ParserResult();
            List<Token> tokens = new List<Token>();
            tokens.Add(tok);
            Position startPos = CurrentToken.StartPos.Copy();

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type is not Constants.TT.IDENTIFIER and not Constants.TT.INT)
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected identifier or int", startPos, CurrentToken.EndPos));
            }

            tokens.Add(CurrentToken);

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Constants.TT.ACCESS_ARROW)
            {
                while (CurrentToken.Type == Constants.TT.ACCESS_ARROW)
                {
                    res.RegisterAdvancement();
                    Advance();

                    if (CurrentToken.Type is not Constants.TT.IDENTIFIER and not Constants.TT.INT)
                    {
                        return res.Failure(new InvalidSyntaxError(
                            "Expected identifier or int", startPos, CurrentToken.EndPos));
                    }

                    tokens.Add(CurrentToken);

                    res.RegisterAdvancement();
                    Advance();
                }
            }

            if (CurrentToken.Type == Constants.TT.EQUALS)
            {
                res.RegisterAdvancement();
                Advance();
                Node expr = res.Register(Expr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(new ObjectAssignNode(tokens, expr));
            }

            return res.Success(new ObjectAccessNode(tokens));
        }

        private ParserResult ListExpr()
        {
            ParserResult res = new ParserResult();
            List<Node> elements = new List<Node>();
            Position startPos = CurrentToken.StartPos.Copy();

            if (CurrentToken.Type != Constants.TT.LBRACKET)
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected '['",
                    CurrentToken.StartPos,
                    CurrentToken.EndPos
                ));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Constants.TT.NEWLINE)
            {
                while (CurrentToken.Type == Constants.TT.NEWLINE)
                {
                    res.RegisterAdvancement();
                    Advance();
                }
            }

            elements.Add(res.Register(Expr()));
            if (res.Error != default(Error))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected ']', 'var', 'if', 'for', 'while', 'define', int, float, identifier, '+', '-', '(', '[' or 'not'",
                    CurrentToken.StartPos, CurrentToken.EndPos));
            }
            else
            {
                while (CurrentToken.Type == Constants.TT.COMMA)
                {
                    res.RegisterAdvancement();
                    Advance();

                    if (CurrentToken.Type == Constants.TT.NEWLINE)
                    {
                        while (CurrentToken.Type == Constants.TT.NEWLINE)
                        {
                            res.RegisterAdvancement();
                            Advance();
                        }
                    }

                    elements.Add(res.Register(Expr()));
                    if (res.Error != default(Error))
                    {
                        return res;
                    }

                    if (CurrentToken.Type == Constants.TT.NEWLINE)
                    {
                        while (CurrentToken.Type == Constants.TT.NEWLINE)
                        {
                            res.RegisterAdvancement();
                            Advance();
                        }
                    }
                }

                res.RegisterAdvancement();
                Advance();
            }

            return res.Success(new ListNode(elements, startPos, CurrentToken.EndPos.Copy()));
        }

        private ParserResult CurlyExpr()
        {
            ParserResult res = new ParserResult();
            Position startPos = CurrentToken.StartPos.Copy();
            Dictionary<Token, Node> elements = new Dictionary<Token, Node>();

            if (CurrentToken.Type != Constants.TT.LCURLY_BRACKET)
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected '{'", startPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Constants.TT.RCURLY_BRACKET)
            {
                res.RegisterAdvancement();
                Advance();
            }
            else
            {
                while (CurrentToken.Type == Constants.TT.NEWLINE)
                {
                    res.RegisterAdvancement();
                    Advance();
                }


                Token varName = CurrentToken;
                if (varName.Type != Constants.TT.IDENTIFIER)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected identifier", startPos, varName.EndPos));
                }

                res.RegisterAdvancement();
                Advance();

                if (CurrentToken.Type != Constants.TT.EQUALS)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected '='", startPos, CurrentToken.EndPos));
                }

                res.RegisterAdvancement();
                Advance();

                Node expr = res.Register(Expr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                elements.Add(varName, expr);

                while (CurrentToken.Type == Constants.TT.COMMA)
                {
                    res.RegisterAdvancement();
                    Advance();

                    Token varName2 = CurrentToken;
                    if (CurrentToken.Type != Constants.TT.IDENTIFIER)
                    {
                        return res.Failure(new InvalidSyntaxError(
                            "Expected identifier", startPos, CurrentToken.EndPos));
                    }

                    res.RegisterAdvancement();
                    Advance();

                    if (CurrentToken.Type != Constants.TT.EQUALS)
                    {
                        return res.Failure(new InvalidSyntaxError(
                            "Expected '='", startPos, CurrentToken.EndPos));
                    }

                    res.RegisterAdvancement();
                    Advance();

                    Node expr2 = res.Register(Expr());
                    if (res.Error != default(Error))
                    {
                        return res;
                    }

                    elements.Add(varName2, expr2);

                    while (CurrentToken.Type == Constants.TT.NEWLINE)
                    {
                        res.RegisterAdvancement();
                        Advance();
                    }
                }

                if (CurrentToken.Type != Constants.TT.RCURLY_BRACKET)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected '}' or ','", startPos, CurrentToken.EndPos));
                }

                res.RegisterAdvancement();
                Advance();
            }

            return res.Success(new ObjectNode(elements, startPos, CurrentToken.EndPos));
        }

        private ParserResult IfExpr(bool head)
        {
            ParserResult res = new ParserResult();
            IfExprBorCNode ifCase =
                (IfExprBorCNode) res.Register(IfExprCase(head ? Constants.HeadKeyword.IF : Constants.Keyword.IF, head));
            if (res.Error != default(Error))
            {
                return res;
            }

            return res.Success(head
                ? new HeadIfNode(ifCase.Cases, ifCase.ElseCase)
                : new IfNode(ifCase.Cases, ifCase.ElseCase));
        }

        private ParserResult IfExprCase(string keyword, bool head)
        {
            ParserResult res = new ParserResult();
            List<IfCase> cases = new List<IfCase>();
            Node elseCase = default;

            if (!CurrentToken.Matches(head ? Constants.TT.HEAD_KEYWORD : Constants.TT.KEYWORD,
                new TokenValue(typeof(string), keyword)))
            {
                return res.Failure(new InvalidSyntaxError(
                    $"Expected '{keyword}'",
                    CurrentToken.StartPos,
                    CurrentToken.EndPos
                ));
            }

            res.RegisterAdvancement();
            Advance();

            Node condition = res.Register(Expr());
            if (res.Error != default(Error))
            {
                return res;
            }

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.THEN)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'then'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Constants.TT.NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();

                Node statements = res.Register(Statements());
                if (res.Error != default(Error))
                {
                    return res;
                }

                cases.Add(new IfCase()
                {
                    Condition = condition,
                    Statements = statements,
                    Bool = true
                });

                if (CurrentToken.Matches(head ? Constants.TT.HEAD_KEYWORD : Constants.TT.KEYWORD,
                    new TokenValue(typeof(string), head ? Constants.HeadKeyword.END : Constants.Keyword.END)))
                {
                    res.RegisterAdvancement();
                    Advance();
                }
                else
                {
                    IfExprBorCNode allCases = (IfExprBorCNode) res.Register(IfExprBorC(head));
                    if (res.Error != default(Error))
                    {
                        return res;
                    }

                    elseCase = allCases.ElseCase;
                    foreach (IfCase casesCase in allCases.Cases)
                    {
                        cases.Add(casesCase);
                    }
                }
            }
            else
            {
                Node expr = res.Register(Statement());
                if (res.Error != default(Error))
                {
                    return res;
                }

                cases.Add(new IfCase()
                {
                    Condition = condition,
                    Statements = expr,
                    Bool = false
                });
                IfExprBorCNode allCases = (IfExprBorCNode) res.Register(IfExprBorC(head));
                elseCase = allCases.ElseCase;
                foreach (IfCase casesCase in allCases.Cases)
                {
                    cases.Add(casesCase);
                }
            }

            return res.Success(new IfExprBorCNode(cases, elseCase));
        }

        private ParserResult IfExprBorC(bool head)
        {
            ParserResult res = new ParserResult();
            List<IfCase> cases = new List<IfCase>();
            Node elseCase;

            if (CurrentToken.Matches(head ? Constants.TT.HEAD_KEYWORD : Constants.TT.KEYWORD,
                new TokenValue(typeof(string), head ? Constants.HeadKeyword.ELSE_IF : Constants.Keyword.ELSE_IF)))
            {
                IfExprBorCNode allCases = (IfExprBorCNode) res.Register(IfExprB(head));
                if (res.Error != default(Error))
                {
                    return res;
                }

                cases = allCases.Cases;
                elseCase = allCases.ElseCase;
            }
            else
            {
                elseCase = res.Register(IfExprC(head));
                if (res.Error != default(Error))
                {
                    return res;
                }
            }

            return res.Success(new IfExprBorCNode(cases, elseCase));
        }

        private ParserResult IfExprB(bool head)
        {
            return IfExprCase(head ? Constants.HeadKeyword.ELSE_IF : Constants.Keyword.ELSE_IF, head);
        }

        private ParserResult IfExprC(bool head)
        {
            ParserResult res = new ParserResult();
            ElseCaseNode elseCase = default;

            if (CurrentToken.Matches(head ? Constants.TT.HEAD_KEYWORD : Constants.TT.KEYWORD,
                new TokenValue(typeof(string), head ? Constants.HeadKeyword.ELSE : Constants.Keyword.ELSE)))
            {
                res.RegisterAdvancement();
                Advance();

                if (CurrentToken.Type == Constants.TT.NEWLINE)
                {
                    res.RegisterAdvancement();
                    Advance();

                    Node statements = res.Register(Statements());
                    if (res.Error != default(Error))
                    {
                        return res;
                    }

                    elseCase = new ElseCaseNode(statements, true);
                    if (CurrentToken.Matches(head ? Constants.TT.HEAD_KEYWORD : Constants.TT.KEYWORD,
                        new TokenValue(typeof(string), head ? Constants.HeadKeyword.END : Constants.Keyword.END)))
                    {
                        res.RegisterAdvancement();
                        Advance();
                    }
                    else
                    {
                        string key = head ? Constants.HeadKeyword.END : Constants.Keyword.END;
                        return res.Failure(new InvalidSyntaxError(
                            $"Expected '{key}'", CurrentToken.StartPos, CurrentToken.EndPos));
                    }
                }
                else
                {
                    Node expr = res.Register(Statement());
                    if (res.Error != default(Error))
                    {
                        return res;
                    }

                    elseCase = new ElseCaseNode(expr, false);
                }
            }

            return res.Success(elseCase);
        }

        private ParserResult WhileExpr()
        {
            ParserResult res = new ParserResult();
            if (!CurrentToken.Matches(Constants.TT.KEYWORD,
                new TokenValue(typeof(string), Constants.Keyword.WHILE)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'while'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            Node condition = res.Register(Expr());
            if (res.Error != default(Error))
            {
                return res;
            }

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.THEN)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'then'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Constants.TT.NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();

                Node body = res.Register(Statements());

                if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.END)))
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected 'el'", CurrentToken.StartPos, CurrentToken.EndPos));
                }

                res.RegisterAdvancement();
                Advance();

                return res.Success(new WhileNode(condition, body, true));
            }

            Node body2 = res.Register(Statement());
            if (res.Error != default(Error))
            {
                return res;
            }

            return res.Success(new WhileNode(condition, body2, false));
        }

        private ParserResult ForExpr()
        {
            ParserResult res = new ParserResult();

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.FOR)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'for'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type != Constants.TT.IDENTIFIER)
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected identifier", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            Token varName = CurrentToken;
            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type != Constants.TT.EQUALS)
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected '='", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            Node startValue = res.Register(Expr());
            if (res.Error != default(Error))
            {
                return res;
            }

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.TO)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected to", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            Node endValue = res.Register(Expr());
            if (res.Error != default(Error))
            {
                return res;
            }

            Node stepValue = default;
            if (CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.STEP)))
            {
                res.RegisterAdvancement();
                Advance();

                stepValue = res.Register(Expr());
                if (res.Error != default(Error))
                {
                    return res;
                }
            }

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.THEN)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'then'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Constants.TT.NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();

                Node body = res.Register(Statements());
                if (res.Error != default(Error))
                {
                    return res;
                }

                if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.END)))
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected 'el'", CurrentToken.StartPos, CurrentToken.EndPos));
                }

                res.RegisterAdvancement();
                Advance();

                return res.Success(new ForNode(varName, startValue, endValue, stepValue, body, true));
            }

            Node body2 = res.Register(Statement());
            if (res.Error != default(Error))
            {
                return res;
            }

            return res.Success(new ForNode(varName, startValue, endValue, stepValue, body2, false));
        }

        private ParserResult FuncExpr()
        {
            ParserResult res = new ParserResult();
            if (!CurrentToken.Matches(Constants.TT.KEYWORD,
                new TokenValue(typeof(string), Constants.Keyword.FUNCTION)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'define'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            Token varName = default;
            if (CurrentToken.Type == Constants.TT.IDENTIFIER)
            {
                varName = CurrentToken;
                res.RegisterAdvancement();
                Advance();
                if (CurrentToken.Type != Constants.TT.LPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected '('", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }
            else
            {
                if (CurrentToken.Type != Constants.TT.LPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected '('", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }

            res.RegisterAdvancement();
            Advance();

            List<Token> argName = new List<Token>();
            if (CurrentToken.Type == Constants.TT.IDENTIFIER)
            {
                argName.Add(CurrentToken);
                res.RegisterAdvancement();
                Advance();

                while (CurrentToken.Type == Constants.TT.COMMA)
                {
                    res.RegisterAdvancement();
                    Advance();

                    if (CurrentToken.Type == Constants.TT.IDENTIFIER)
                    {
                        return res.Failure(new InvalidSyntaxError(
                            "Expected identifier", CurrentToken.StartPos, CurrentToken.EndPos));
                    }

                    argName.Add(CurrentToken);
                    res.RegisterAdvancement();
                    Advance();
                }

                if (CurrentToken.Type != Constants.TT.RPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected identifier or ')'", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }
            else
            {
                if (CurrentToken.Type != Constants.TT.RPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected identifier or ')'", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Constants.TT.ARROW)
            {
                res.RegisterAdvancement();
                Advance();

                Node body = res.Register(Expr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(new FunctionDefineNode(varName, argName, body, true));
            }

            if (CurrentToken.Type != Constants.TT.NEWLINE)
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected '=>' or NEWLINE", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            Node body2 = res.Register(Statements());
            if (res.Error != default(Error))
            {
                return res;
            }

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.END)))
            {
                return res.Failure(new InvalidSyntaxError("Expected 'el'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            return res.Success(new FunctionDefineNode(varName, argName, body2, false));
        }

        private ParserResult ClassExpr()
        {
            ParserResult res = new ParserResult();
            if (!CurrentToken.Matches(Constants.TT.KEYWORD,
                new TokenValue(typeof(string), Constants.Keyword.CLASS)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'class'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            Token className = CurrentToken;
            if (className.Type != Constants.TT.IDENTIFIER)
            {
                return res.Failure(new InvalidSyntaxError("Expected identifier", className.StartPos, className.EndPos));
            }

            res.RegisterAdvancement();
            Advance();


            while (CurrentToken.Type == Constants.TT.NEWLINE)
            {
                res.RegisterAdvancement();
                Advance();
            }


            List<Node> items = new List<Node>();

            while (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.END)))
            {
                if (CurrentToken.Type == Constants.TT.NEWLINE)
                {
                    res.RegisterAdvancement();
                    Advance();
                    continue;
                }

                if (CurrentToken.Matches(Constants.TT.KEYWORD,
                    new TokenValue(typeof(string), Constants.Keyword.METHOD)))
                {
                    items.Add(res.Register(MethodExpr()));
                    if (res.Error != default(Error))
                    {
                        return res;
                    }
                }
                else if (CurrentToken.Matches(Constants.TT.KEYWORD,
                    new TokenValue(typeof(string), Constants.Keyword.FIELD)))
                {
                    items.Add(res.Register(FieldExpr()));
                    if (res.Error != default(Error))
                    {
                        return res;
                    }
                }
                else
                {
                    return res.Failure(new InvalidSyntaxError("Expected 'field' or 'method'", CurrentToken.StartPos,
                        CurrentToken.EndPos));
                }
            }

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.END)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'el'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            return res.Success(new ClassNode(className, items));
        }

        private ParserResult ClassConstructExpr()
        {
            ParserResult res = new ParserResult();
            res.RegisterAdvancement();
            Advance();

            Node expr = res.Register(Atom());
            if (res.Error != default(Error))
            {
                return res;
            }

            if (CurrentToken.Type != Constants.TT.LPAREN)
            {
                return res.Failure(new InvalidSyntaxError("Expected ')'", CurrentToken.StartPos,
                    CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            List<Node> argNodes = new List<Node>();
            if (CurrentToken.Type == Constants.TT.RPAREN)
            {
                res.RegisterAdvancement();
                Advance();
            }
            else
            {
                argNodes.Add(res.Register(Expr()));
                if (res.Error != default(Error))
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected ')', 'let', 'if', 'for', 'while', 'define', int, float, identifier, '+', '-', '(', '[' or '!'",
                        CurrentToken.StartPos, CurrentToken.EndPos));
                }

                while (CurrentToken.Type == Constants.TT.COMMA)
                {
                    res.RegisterAdvancement();
                    Advance();

                    argNodes.Add(res.Register(Expr()));
                    if (res.Error != default(Error))
                    {
                        return res;
                    }
                }

                if (CurrentToken.Type != Constants.TT.RPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected ')' or ','",
                        CurrentToken.StartPos, CurrentToken.EndPos));
                }

                res.RegisterAdvancement();
                Advance();
            }

            return res.Success(new ClassConstructorNode(expr, argNodes));
        }

        private ParserResult FieldExpr()
        {
            ParserResult res = new ParserResult();

            res.RegisterAdvancement();
            Advance();

            Token varName = CurrentToken;
            if (CurrentToken.Type != Constants.TT.IDENTIFIER)
            {
                return res.Failure(new InvalidSyntaxError("Expected identifier", CurrentToken.StartPos,
                    CurrentToken.EndPos));
            }

            Node expr = res.Register(Expr());
            if (res.Error != default(Error))
            {
                return res;
            }

            return res.Success(new FieldNode(varName, expr));
        }

        private ParserResult MethodExpr()
        {
            ParserResult res = new ParserResult();
            if (!CurrentToken.Matches(Constants.TT.KEYWORD,
                new TokenValue(typeof(string), Constants.Keyword.METHOD)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected 'method'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            Token varName = default;
            if (CurrentToken.Type == Constants.TT.IDENTIFIER)
            {
                varName = CurrentToken;
                res.RegisterAdvancement();
                Advance();
                if (CurrentToken.Type != Constants.TT.LPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected '('", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }
            else
            {
                if (CurrentToken.Type != Constants.TT.LPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected '('", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }

            res.RegisterAdvancement();
            Advance();

            List<Token> argName = new List<Token>();
            if (CurrentToken.Type == Constants.TT.IDENTIFIER)
            {
                argName.Add(CurrentToken);
                res.RegisterAdvancement();
                Advance();

                while (CurrentToken.Type == Constants.TT.COMMA)
                {
                    res.RegisterAdvancement();
                    Advance();

                    if (CurrentToken.Type == Constants.TT.IDENTIFIER)
                    {
                        return res.Failure(new InvalidSyntaxError(
                            "Expected identifier", CurrentToken.StartPos, CurrentToken.EndPos));
                    }

                    argName.Add(CurrentToken);
                    res.RegisterAdvancement();
                    Advance();
                }

                if (CurrentToken.Type != Constants.TT.RPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected identifier or ')'", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }
            else
            {
                if (CurrentToken.Type != Constants.TT.RPAREN)
                {
                    return res.Failure(new InvalidSyntaxError(
                        "Expected identifier or ')'", CurrentToken.StartPos, CurrentToken.EndPos));
                }
            }

            res.RegisterAdvancement();
            Advance();

            if (CurrentToken.Type == Constants.TT.ARROW)
            {
                res.RegisterAdvancement();
                Advance();

                Node body = res.Register(Expr());
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(new MethodDefineNode(varName, argName, body, true));
            }

            if (CurrentToken.Type != Constants.TT.NEWLINE)
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected '=>' or NEWLINE", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            Node body2 = res.Register(Statements());
            if (res.Error != default(Error))
            {
                return res;
            }

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.END)))
            {
                return res.Failure(new InvalidSyntaxError("Expected 'el'", CurrentToken.StartPos, CurrentToken.EndPos));
            }

            res.RegisterAdvancement();
            Advance();

            return res.Success(new MethodDefineNode(varName, argName, body2, false));
        }

        private ParserResult ExportExpr()
        {
            ParserResult res = new ParserResult();
            Position startPos = CurrentToken.StartPos.Copy();

            res.RegisterAdvancement();
            Advance();

            if (!CurrentToken.Matches(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.FUNCTION)))
            {
                return res.Failure(new InvalidSyntaxError(
                    "Expected define", startPos, CurrentToken.EndPos));
            }

            Node expr = res.Register(Expr());
            if (res.Error != default(Error))
            {
                return res;
            }

            return res.Success(new ExportNode(startPos, CurrentToken.EndPos, expr));
        }

        private ParserResult HeadExpr()
        {
            ParserResult res = new ParserResult();
            Token tok = CurrentToken;
            Position startPos = tok.StartPos.Copy();

            if (tok.Matches(Constants.TT.HEAD_KEYWORD, new TokenValue(typeof(string), Constants.HeadKeyword.IMPORT)))
            {
                res.RegisterAdvancement();
                Advance();

                if (CurrentToken.Type != Constants.TT.STRING)
                {
                    return res.Failure(new InvalidSyntaxError("Expected string", startPos, CurrentToken.EndPos));
                }

                Token path = CurrentToken;
                res.RegisterAdvancement();
                Advance();

                return res.Success(new ImportNode(startPos, CurrentToken.EndPos, path));
            }
            else if (tok.Matches(Constants.TT.HEAD_KEYWORD,
                new TokenValue(typeof(string), Constants.HeadKeyword.IF)))
            {
                Node ifExpr = res.Register(IfExpr(true));
                if (res.Error != default(Error))
                {
                    return res;
                }

                return res.Success(ifExpr);
            }

            return res.Failure(new InvalidSyntaxError(
                $"Undefined head statement '{tok.Value}'", startPos, CurrentToken.EndPos));
        }

        private bool BinOpCheckOperators(Token[] operators)
        {
            foreach (Token token in operators)
            {
                if (CurrentToken.Type == token.Type && CurrentToken.Value.Matches(token.Value))
                {
                    return true;
                }
            }

            return false;
        }

        private ParserResult BinOp(Func<ParserResult> left, Token[] operators, Func<ParserResult> right)
        {
            ParserResult res = new ParserResult();
            Node leftNode = res.Register(left());
            if (res.Error != default(Error))
            {
                return res;
            }

            Token opToken;
            NumberNode rightNode;
            while (BinOpCheckOperators(operators))
            {
                opToken = CurrentToken;
                res.RegisterAdvancement();
                Advance();
                rightNode = (NumberNode) res.Register(right());
                if (res.Error != default(Error))
                {
                    return res;
                }

                leftNode = new BinOpNode(leftNode, opToken, rightNode);
            }

            return res.Success(leftNode);
        }

        private ParserResult BinOp(Func<ParserResult> left, String[] operators, Func<ParserResult> right)
        {
            ParserResult res = new ParserResult();
            Node leftNode = res.Register(left());
            if (res.Error != default(Error))
            {
                return res;
            }

            Token opToken;
            Node rightNode;
            while (operators.Contains(CurrentToken.Type))
            {
                opToken = CurrentToken;
                res.RegisterAdvancement();
                Advance();
                rightNode = res.Register(right());
                if (res.Error != default(Error))
                {
                    return res;
                }

                leftNode = new BinOpNode(leftNode, opToken, rightNode);
            }

            return res.Success(leftNode);
        }

        private ParserResult BinOp(Func<ParserResult> left, Token[] operators)
        {
            return BinOp(left, operators, left);
        }

        private ParserResult BinOp(Func<ParserResult> left, String[] operators)
        {
            return BinOp(left, operators, left);
        }
    }
}
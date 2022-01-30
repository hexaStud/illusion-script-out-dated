using System;
using System.Collections.Generic;
using IllusionScript.SDK;
using IllusionScript.SDK.Bundler;
using IllusionScript.SDK.Nodes;
using IllusionScript.SDK.Nodes.Assets;

namespace IllusionScript.SDK
{
    public abstract class Node
    {
        public Position StartPos;
        public Position EndPos;

        protected Node(Position startPos, Position endPos)
        {
            StartPos = startPos;
            EndPos = endPos;
        }

        public abstract string __repr__();
        public abstract string __bundle__();

        public abstract Node __unbundle__(Json json);

        public static Node ConvertNode(Json json)
        {
            string type = json.GetAsText("type");

            switch (type)
            {
                case "BinOpNode":
                    return new BinOpNode(Empty(), Token.Empty(), Empty())
                        .__unbundle__(json);
                case "BreakNode":
                    return new BreakNode(Position.Empty(), Position.Empty()).__unbundle__(json);
                case "CallNode":
                    return new CallNode(Empty(), new List<Node>()).__unbundle__(json);
                case "ClassConstructorNode":
                    return new ClassConstructorNode(Empty(), new List<Node>()).__unbundle__(json);
                case "ClassNode":
                    return new ClassNode(Token.Empty(), new List<Node>(), new List<Node>(), default)
                        .__unbundle__(json);
                case "ConstAssignNode":
                    return new ConstAssignNode(Token.Empty(), Empty()).__unbundle__(json);
                case "ContinueNode":
                    return new ContinueNode(Position.Empty(), Position.Empty()).__unbundle__(json);
                case "ElseCaseNode":
                    return new ElseCaseNode(Empty(), false).__unbundle__(json);
                case "EmptyNode":
                    return Empty();
                case "ExportNode":
                    return new ExportNode(Position.Empty(), Position.Empty(), Empty(), "").__unbundle__(json);
                case "FieldNode":
                    return new FieldNode(Token.Empty(), Token.Empty(), Empty()).__unbundle__(json);
                case "ForNode":
                    return new ForNode(Token.Empty(), Empty(), Empty(), Empty(), Empty(), false).__unbundle__(json);
                case "FunctionDefineNode":
                    return new FunctionDefineNode(Token.Empty(), new List<Token>(), Empty(), false).__unbundle__(json);
                case "HeadIfNode":
                    return new HeadIfNode(new List<IfCase>(), Empty()).__unbundle__(json);
                case "IfExprBorCNode":
                    return new IfExprBorCNode(new List<IfCase>(), Empty()).__unbundle__(json);
                case "IfNode":
                    return new IfNode(new List<IfCase>(), Empty()).__unbundle__(json);
                case "ImportNode":
                    return new ImportNode(Position.Empty(), Position.Empty(), Token.Empty()).__unbundle__(json);
                case "ListNode":
                    return new ListNode(new List<Node>(), Position.Empty(), Position.Empty()).__unbundle__(json);
                case "MainNode":
                    return new MainNode().__unbundle__(json);
                case "MethodDefineNode":
                    return new MethodDefineNode(Token.Empty(), Token.Empty(), new List<Token>(), Empty(), false)
                        .__unbundle__(json);
                case "NullNode":
                    return new NullNode(Position.Empty(), Position.Empty()).__unbundle__(json);
                case "NumberNode":
                    return new NumberNode(Token.Empty());
                case "ObjectAccessNode":
                    return new ObjectAccessNode(new List<Token>()).__unbundle__(json);
                case "ObjectAssignNode":
                    return new ObjectAssignNode(new List<Token>(), Empty()).__unbundle__(json);
                case "ObjectCallNode":
                    return new ObjectCallNode(Empty(), new List<Node>());
                case "ObjectNode":
                    return new ObjectNode(new Dictionary<Token, Node>(), Position.Empty(), Position.Empty());
                case "ReturnNode":
                    return new ReturnNode(Empty(), Position.Empty(), Position.Empty()).__unbundle__(json);
                case "StringNode":
                    return new StringNode(Token.Empty()).__unbundle__(json);
                case "UnaryOpNode":
                    return new UnaryOpNode(Token.Empty(), Empty()).__unbundle__(json);
                case "VarAccessNode":
                    return new VarAccessNode(Token.Empty()).__unbundle__(json);
                case "VarAssignNode":
                    return new VarAssignNode(Token.Empty(), Empty(), false).__unbundle__(json);
                case "WhileNode":
                    return new WhileNode(Empty(), Empty(), false).__unbundle__(json);
                default:
                    throw new Exception($"'{type}' is not implemented");
            }
        }

        private static Node Empty()
        {
            return new EmptyNode();
        }
    }
}
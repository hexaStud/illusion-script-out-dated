using System;
using System.IO;
using System.Collections.Generic;
using IllusionScript.SDK.Bundler;
using IllusionScript.SDK.Errors;
using IllusionScript.SDK.Nodes;
using IllusionScript.SDK.Nodes.Assets;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK
{
    public class Interpreter
    {
        public static List<string> Argv = new();
        public static MemoryCash MemoryCash = new();
        public Dictionary<string, Value> Exports;

        public Interpreter()
        {
            Exports = new Dictionary<string, Value>();
        }

        public RuntimeResult Visit(Node node, Context context, bool editContext = true)
        {
            switch (node.GetType().Name)
            {
                case "BinOpNode":
                    return VisitBinOpNode((BinOpNode)node, context);
                case "UnaryOpNode":
                    return VisitUnaryOpNode((UnaryOpNode)node, context);
                case "NumberNode":
                    return VisitNumberNode((NumberNode)node, context);
                case "StringNode":
                    return VisitStringNode((StringNode)node, context);
                case "NullNode":
                    return VisitNullNode((NullNode)node, context);
                case "VarAccessNode":
                    return VisitVarAccessNode((VarAccessNode)node, context);
                case "VarAssignNode":
                    return VisitVarAssignNode((VarAssignNode)node, context, editContext);
                case "ConstAssignNode":
                    return VisitConstAssignNode((ConstAssignNode)node, context);
                case "FieldNode":
                    return VisitFieldNode((FieldNode)node, context);
                case "IfNode":
                    return VisitIfNode((IfNode)node, context);
                case "ForNode":
                    return VisitForNode((ForNode)node, context);
                case "WhileNode":
                    return VisitWhileNode((WhileNode)node, context);
                case "FunctionDefineNode":
                    return VisitFunctionDefineNode((FunctionDefineNode)node, context);
                case "MethodDefineNode":
                    return VisitMethodDefineNode((MethodDefineNode)node, context);
                case "ClassNode":
                    return VisitClassNode((ClassNode)node, context);
                case "ClassConstructorNode":
                    return VisitClassConstructorNode((ClassConstructorNode)node, context);
                case "CallNode":
                    return VisitCallNode((CallNode)node, context);
                case "ObjectCallNode":
                    return VisitObjectCallNode((ObjectCallNode)node, context);
                case "ListNode":
                    return VisitListNode((ListNode)node, context);
                case "ObjectNode":
                    return VisitObjectNode((ObjectNode)node, context);
                case "ObjectAccessNode":
                    return VisitObjectAccessNode((ObjectAccessNode)node, context);
                case "ObjectAssignNode":
                    return VisitObjectAssignNode((ObjectAssignNode)node, context);
                case "ReturnNode":
                    return VisitReturnNode((ReturnNode)node, context);
                case "ContinueNode":
                    return VisitContinueNode((ContinueNode)node, context);
                case "BreakNode":
                    return VisitBreakNode((BreakNode)node, context);
                case "ImportNode":
                    return VisitImportNode((ImportNode)node, context);
                case "UseNode":
                    return VisitUseNode((UseNode)node, context);
                case "PackageNode":
                    return VisitPackageNode((PackageNode)node, context);
                case "HeadIfNode":
                    return VisitHeadIfNode((HeadIfNode)node, context);
                case "ExportNode":
                    return VisitExportNode((ExportNode)node, context);
                case "MainNode":
                    return VisitMainNode((MainNode)node, context);
                default:
                    throw NoVisitMethod(node, context);
            }
        }

        private Exception NoVisitMethod(Node node, Context context)
        {
            throw new Exception($"No Visit{node.GetType().Name} method defined");
        }

        private RuntimeResult VisitBinOpNode(BinOpNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Value left = res.Register(Visit(node.LeftNode, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            Value right = res.Register(Visit(node.RightNode, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            Error error = default;
            Value result = default;

            if (node.OpToken.Type == Constants.TT.PLUS)
            {
                Tuple<Error, Value> data = left.AddedTo(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.MINUS)
            {
                Tuple<Error, Value> data = left.SubbedBy(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.MUL)
            {
                Tuple<Error, Value> data = left.MultedBy(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.DIV)
            {
                Tuple<Error, Value> data = left.DivedBy(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.MODULO)
            {
                Tuple<Error, Value> data = left.ModuloedBy(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.POW)
            {
                Tuple<Error, Value> data = left.PowedBy(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.DOUBLE_EQUALS)
            {
                Tuple<Error, Value> data = left.GetComparisonEQ(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.NOT_EQUALS)
            {
                Tuple<Error, Value> data = left.GetComparisonNEQ(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.LESS_THAN)
            {
                Tuple<Error, Value> data = left.GetComparisonLT(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.LESS_EQUALS)
            {
                Tuple<Error, Value> data = left.GetComparisonLEQ(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.GREATER_THAN)
            {
                Tuple<Error, Value> data = left.GetComparisonGT(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.TT.GREATER_EQUALS)
            {
                Tuple<Error, Value> data = left.GetComparisonGEQ(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.Keyword.AND)
            {
                Tuple<Error, Value> data = left.AndedBy(right);
                error = data.Item1;
                result = data.Item2;
            }
            else if (node.OpToken.Type == Constants.Keyword.OR)
            {
                Tuple<Error, Value> data = left.OredBy(right);
                error = data.Item1;
                result = data.Item2;
            }

            if (error != default(Error))
            {
                return res.Failure(error);
            }
            else
            {
                result.SetPosition(node.StartPos, node.EndPos);
                return res.Success(result);
            }
        }

        private RuntimeResult VisitUnaryOpNode(UnaryOpNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Value number = res.Register(Visit(node.Node, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            Error error = default;

            if (node.OpToken.Type == Constants.TT.MINUS)
            {
                Tuple<Error, Value> data = number.MultedBy(new NumberValue(new TokenValue(typeof(int), "-1")));
                error = data.Item1;
                number = data.Item2;
            }
            else if (node.OpToken.Matches(Constants.TT.KEYWORD,
                         new TokenValue(typeof(string), Constants.Keyword.NOT)))
            {
                Tuple<Error, Value> data = number.Notted();
                error = data.Item1;
                number = data.Item2;
            }

            if (error != default(Error))
            {
                return res.Failure(error);
            }

            number.SetPosition(node.StartPos, node.EndPos);
            return res.Success(number);
        }

        private RuntimeResult VisitNumberNode(NumberNode node, Context context)
        {
            string numStr = node.Token.Value.Value;

            return new RuntimeResult().Success(
                new NumberValue(new TokenValue(numStr.Contains(".") ? typeof(float) : typeof(int), numStr))
                    .SetContext(context).SetPosition(node.StartPos, node.EndPos));
        }

        private RuntimeResult VisitStringNode(StringNode node, Context context)
        {
            return new RuntimeResult().Success(
                new StringValue(new TokenValue(typeof(string), node.Token.Value.Value))
                    .SetContext(context).SetPosition(node.StartPos, node.EndPos));
        }

        private RuntimeResult VisitNullNode(NullNode node, Context context)
        {
            return new RuntimeResult().Success(NumberValue.Null.SetContext(context)
                .SetPosition(node.StartPos, node.EndPos));
        }

        private RuntimeResult VisitVarAccessNode(VarAccessNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string varName = node.Token.Value.GetAsString();
            SymbolTableValue symbolTableValue = context.SymbolTable.Get(varName);

            if (symbolTableValue.Value == default(Value))
            {
                return res.Failure(new RuntimeError($"'{varName}' is not defined", context, node.StartPos,
                    node.EndPos));
            }

            Value value = symbolTableValue.Value;
            value = value.Copy();
            if (value.IsBuiltIn())
            {
                value.SetContext(context).SetPosition(node.StartPos, node.EndPos);
            }

            return res.Success(value);
        }

        private RuntimeResult VisitVarAssignNode(VarAssignNode node, Context context, bool editContext)
        {
            RuntimeResult res = new RuntimeResult();
            string varName = node.Token.Value.GetAsString();
            Value value = res.Register(Visit(node.Node, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            if (editContext)
            {
                if (context.SymbolTable.IsConstants(varName))
                {
                    return res.Failure(new RuntimeError(
                        "Cannot re-declare constants", context, node.StartPos, node.EndPos));
                }

                if (node.DeclareNew)
                {
                    if (!context.SymbolTable.HasInCurrent(varName))
                    {
                        context.SymbolTable.Set(varName, value);
                    }
                    else
                    {
                        return res.Failure(new RuntimeError(
                            $"Symbol with the name '{varName}' already exists in this context", context, node.StartPos,
                            node.EndPos));
                    }
                }
                else
                {
                    if (context.SymbolTable.HasInAll(varName))
                    {
                        context.SymbolTable.Update(varName, value);
                    }
                    else
                    {
                        return res.Failure(new RuntimeError(
                            $"'{varName}' is not defined", context, node.StartPos, node.EndPos));
                    }
                }
            }

            return res.Success(value);
        }

        private RuntimeResult VisitConstAssignNode(ConstAssignNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string varName = node.Token.Value.GetAsString();
            Value value = res.Register(Visit(node.Node, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            if (context.SymbolTable.HasInCurrent(varName))
            {
                return res.Failure(new RuntimeError(
                    "Cannot re-declare constants", context, node.StartPos, node.EndPos));
            }

            context.SymbolTable.Set(varName, value, true);
            return res.Success(value);
        }

        private RuntimeResult VisitFieldNode(FieldNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Value value = res.Register(Visit(node.Node, context, false));
            if (res.ShouldReturn())
            {
                return res;
            }

            return res.Success(new FieldValue(node.ContextIsolation, node.Token.Value.GetAsString(), value));
        }

        private RuntimeResult VisitIfNode(IfNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            foreach (IfCase nodeCase in node.Cases)
            {
                bool shouldReturnNull = nodeCase.Bool;
                Value conditionValue = res.Register(Visit(nodeCase.Condition, context));
                if (res.ShouldReturn())
                {
                    return res;
                }

                if (conditionValue.IsTrue())
                {
                    Value exprValue = res.Register(Visit(nodeCase.Statements, context));
                    if (res.ShouldReturn())
                    {
                        return res;
                    }

                    return res.Success((shouldReturnNull) ? NumberValue.Null : exprValue);
                }
            }

            if (node.ElseCase != default(ElseCaseNode))
            {
                Value elseValue = res.Register(Visit(node.ElseCase.Statements, context));
                if (res.ShouldReturn())
                {
                    return res;
                }

                return res.Success((node.ElseCase.Bool) ? NumberValue.Null : elseValue);
            }

            return res.Success(NumberValue.Null);
        }

        private RuntimeResult VisitForNode(ForNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Value> elements = new List<Value>();

            NumberValue startValue = (NumberValue)res.Register(Visit(node.StartValue, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            NumberValue endValue = (NumberValue)res.Register(Visit(node.EndValue, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            NumberValue stepValue = new NumberValue(new TokenValue(typeof(int), "1"));

            if (node.StepValue != default(Node))
            {
                stepValue = (NumberValue)res.Register(Visit(node.StepValue, context));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }

            float i = startValue.Value.GetAsFloat();
            Func<float, bool> condition = x => x > endValue.Value.GetAsFloat();

            if (stepValue.Value.GetAsFloat() >= 0)
            {
                condition = x => x < endValue.Value.GetAsFloat();
            }

            while (condition(i))
            {
                if (context.SymbolTable.IsConstants(node.VarName.Value.GetAsString()))
                {
                    return res.Failure(new RuntimeError("Cannot re-declare constants", context, node.StartPos,
                        node.EndPos));
                }

                string iStr = i.ToString();
                context.SymbolTable.Set(node.VarName.Value.GetAsString(),
                    new NumberValue(new TokenValue(iStr.Contains(".") ? typeof(float) : typeof(int), iStr)));
                i += stepValue.Value.GetAsFloat();
                Context newContext = new Context("Loop[For]", context, node.StartPos)
                {
                    SymbolTable = new SymbolTable(context.SymbolTable)
                };
                Value value = res.Register(Visit(node.Body, newContext));
                if (res.ShouldReturn())
                {
                    return res;
                }

                if (res.LoopShouldContinue)
                {
                    continue;
                }

                if (res.LoopShouldBreak)
                {
                    break;
                }

                elements.Add(value);
            }

            return res.Success(node.ShouldReturnNull
                ? NumberValue.Null
                : new ListValue(elements).SetContext(context).SetPosition(node.StartPos, node.EndPos));
        }

        private RuntimeResult VisitWhileNode(WhileNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Value> elements = new List<Value>();

            while (true)
            {
                Value condition = res.Register(Visit(node.Condition, context));
                if (res.ShouldReturn())
                {
                    return res;
                }

                if (!condition.IsTrue())
                {
                    break;
                }

                Context newContext = new Context("Loop[While]", context, node.StartPos)
                {
                    SymbolTable = new SymbolTable(context.SymbolTable)
                };
                Value value = res.Register(Visit(node.Body, newContext));
                if (res.ShouldReturn())
                {
                    return res;
                }

                if (res.LoopShouldContinue)
                {
                    continue;
                }

                if (res.LoopShouldBreak)
                {
                    break;
                }

                elements.Add(value);
            }

            return res.Success(node.ShouldReturnNull
                ? NumberValue.Null
                : new ListValue(elements).SetContext(context).SetPosition(node.StartPos, node.EndPos));
        }

        private RuntimeResult VisitFunctionDefineNode(FunctionDefineNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string funcName = default;
            if (node.VarName != default(Token))
            {
                funcName = node.VarName.Value.GetAsString();
            }

            Node bodyNode = node.Body;
            List<string> argNames = new List<string>();
            foreach (Token token in node.ArgName)
            {
                argNames.Add(token.Value.GetAsString());
            }

            Value funcValue = new FunctionValue(funcName, bodyNode, argNames, node.ShouldAutoReturn).SetContext(context)
                .SetPosition(node.StartPos, node.EndPos);
            if (node.VarName != default(Token))
            {
                if (context.SymbolTable.IsConstants(node.VarName.Value.GetAsString()))
                {
                    return res.Failure(new RuntimeError(
                        "Cannot re-declare constants", context, node.StartPos, node.EndPos));
                }

                context.SymbolTable.Set(funcName, funcValue);
            }

            return res.Success(funcValue);
        }

        private RuntimeResult VisitMethodDefineNode(MethodDefineNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            string funcName = default;
            if (node.VarName != default(Token))
            {
                funcName = node.VarName.Value.GetAsString();
            }

            Node bodyNode = node.Body;
            List<string> argNames = new List<string>();
            foreach (Token token in node.ArgName)
            {
                argNames.Add(token.Value.GetAsString());
            }

            Value funcValue =
                new MethodValue(node.ContextIsolation, funcName, bodyNode, argNames, node.ShouldAutoReturn)
                    .SetContext(context)
                    .SetPosition(node.StartPos, node.EndPos);

            return res.Success(funcValue);
        }

        private RuntimeResult VisitClassNode(ClassNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();

            string className = node.Name.Value.GetAsString();
            List<ClassItemValue> fields = new List<ClassItemValue>();
            foreach (Node field in node.Fields)
            {
                if (field.GetType() != typeof(MethodDefineNode) && field.GetType() != typeof(FieldNode))
                {
                    return res.Failure(new RuntimeError(
                        "Can only declare methods and fields in classes", context, field.StartPos, field.EndPos));
                }

                fields.Add((ClassItemValue)res.Register(Visit(field, context, false)));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }

            List<ClassItemValue> staticFields = new List<ClassItemValue>();
            foreach (Node field in node.StaticFields)
            {
                if (field.GetType() != typeof(MethodDefineNode) && field.GetType() != typeof(FieldNode))
                {
                    return res.Failure(new RuntimeError(
                        "Can only declare methods and fields in classes", context, field.StartPos, field.EndPos));
                }

                staticFields.Add((ClassItemValue)res.Register(Visit(field, context, false)));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }

            ClassValue value = default;
            if (node.Extends != default(Token))
            {
                string name = node.Extends.Value.GetAsString();
                if (context.SymbolTable.HasInAll(name))
                {
                    Value extender = context.SymbolTable.Get(name).Value;
                    if (extender.GetType() != typeof(ClassValue))
                    {
                        return res.Failure(new RuntimeError($"Extends value '{name}' is not a class", context,
                            node.StartPos, node.EndPos));
                    }

                    value = (ClassValue)extender;
                }
                else
                {
                    return res.Failure(
                        new RuntimeError($"'{name}' is not defined", context, node.StartPos, node.EndPos));
                }
            }


            Value classValue = new ClassValue(className, fields, staticFields, value).SetContext(context)
                .SetPosition(node.StartPos, node.EndPos);

            if (context.SymbolTable.IsConstants(className))
            {
                return res.Failure(new RuntimeError(
                    "Cannot re-declare constants", context, node.StartPos, node.EndPos));
            }

            context.SymbolTable.Set(className, classValue);

            return res.Success(classValue);
        }

        private RuntimeResult VisitClassConstructorNode(ClassConstructorNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Value value = res.Register(Visit(node.ClassName, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            if (value.GetType() != typeof(ClassValue))
            {
                return res.Failure(new RuntimeError(
                    $"Can only construct class not '{value.__repr__(0)}'", context, node.StartPos, node.EndPos));
            }

            ClassValue classValue = (ClassValue)value;
            value = res.Register(classValue.Construct(new List<Value>())).SetPosition(node.StartPos, node.EndPos);
            if (classValue.Constructor != default(MethodValue))
            {
                res.Register(classValue.Constructor.Execute(classValue.ConstructorArgs, value));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }

            return res.Success(value);
        }

        private RuntimeResult VisitCallNode(CallNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Value> args = new List<Value>();

            Value funcValue = res.Register(Visit(node.Node, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            Value call = funcValue.Copy().SetPosition(node.StartPos, node.EndPos);
            foreach (Node node1 in node.ArgNode)
            {
                args.Add(res.Register(Visit(node1, context)));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }

            Value returnValue = res.Register(call.Execute(args));
            if (res.ShouldReturn())
            {
                return res;
            }

            returnValue = returnValue.Copy().SetContext(context).SetPosition(node.StartPos, node.EndPos);
            return res.Success(returnValue);
        }

        private RuntimeResult VisitObjectCallNode(ObjectCallNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Value> args = new List<Value>();
            ObjectAccessNode funcNode = (ObjectAccessNode)node.Node;

            Value funcValue = res.Register(Visit(funcNode, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            if (funcValue.GetType() == typeof(MethodValue))
            {
                MethodValue method = (MethodValue)funcValue;
                if (method.ContextIsolation.Matches(Constants.TT.KEYWORD,
                        new TokenValue(typeof(string), Constants.Keyword.PRIVATE)) && !funcNode.Tokens[0]
                        .Matches(Constants.TT.IDENTIFIER, new TokenValue(typeof(string), Constants.Keyword.THIS)))
                {
                    return res.Failure(new RuntimeError(
                        $"Cannot access private method '{method.Name}' with outer scope", context,
                        node.StartPos,
                        node.EndPos));
                }
            }

            Value dataObj = res.Register(Visit(new VarAccessNode(funcNode.Tokens[0]), context));
            if (res.ShouldReturn())
            {
                return res;
            }

            Value call = funcValue.Copy().SetPosition(node.StartPos, node.EndPos);
            foreach (Node node1 in node.ArgNode)
            {
                args.Add(res.Register(Visit(node1, context)));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }

            Value returnValue = res.Register(call.Execute(args, dataObj));
            if (res.ShouldReturn())
            {
                return res;
            }

            returnValue = returnValue.Copy().SetContext(context).SetPosition(node.StartPos, node.EndPos);
            return res.Success(returnValue);
        }

        private RuntimeResult VisitListNode(ListNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Value> elements = new List<Value>();
            foreach (Node nodeElement in node.Elements)
            {
                elements.Add(res.Register(Visit(nodeElement, context)));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }

            return res.Success(new ListValue(elements).SetContext(context).SetPosition(node.StartPos, node.EndPos));
        }

        private RuntimeResult VisitObjectNode(ObjectNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Dictionary<string, Value> elements = new Dictionary<string, Value>();

            foreach (KeyValuePair<Token, Node> nodeElement in node.Elements)
            {
                elements[nodeElement.Key.Value.GetAsString()] = res.Register(Visit(nodeElement.Value, context));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }

            return res.Success(new ObjectValue(elements).SetContext(context).SetPosition(node.StartPos, node.EndPos));
        }

        private RuntimeResult VisitObjectAccessNode(ObjectAccessNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Token> tokens = new List<Token>(node.Tokens);
            Token current = tokens[0];
            tokens.RemoveAt(0);
            Token start = current;
            Value variable = res.Register(Visit(new VarAccessNode(current), context));
            if (variable.GetType() == typeof(ClassValue))
            {
                ClassValue c = (ClassValue)variable;
                variable = c.StaticObject;
            }

            if (res.ShouldReturn())
            {
                return res;
            }

            if (tokens.Count != 0)
            {
                while (tokens.Count != 0)
                {
                    current = tokens[0];
                    tokens.RemoveAt(0);

                    if (variable.GetType() == typeof(ObjectValue))
                    {
                        ObjectValue rr = (ObjectValue)variable;
                        if (!rr.Elements.ContainsKey(current.Value.GetAsString()))
                        {
                            return res.Failure(new RuntimeError(
                                $"'{current.Value.GetAsString()}' is not defined in the object '{start.Value.GetAsString()}'",
                                context, node.StartPos, node.EndPos));
                        }

                        variable = rr.Elements[current.Value.GetAsString()];
                    }
                    else if (variable.GetType() == typeof(ListValue))
                    {
                        if (current.Type != Constants.TT.INT)
                        {
                            return res.Failure(new RuntimeError("Expected int", context, node.StartPos, node.EndPos));
                        }

                        ListValue rr = (ListValue)variable;

                        int index = current.Value.GetAsInt();
                        if (index < 0 || index > rr.Elements.Count - 1)
                        {
                            return res.Failure(new RuntimeError("Int is out of bounds of the list", context,
                                node.StartPos, node.EndPos));
                        }

                        variable = rr.Elements[index];
                    }

                    if (tokens.Count != 0 && variable.GetType() != typeof(ObjectValue) &&
                        variable.GetType() != typeof(ListValue))
                    {
                        return res.Failure(new RuntimeError($"Cannot access on '{variable.__repr__(0)}'", context,
                            node.StartPos, node.EndPos));
                    }
                }
            }

            if (variable.GetType() == typeof(MethodValue))
            {
                MethodValue method = (MethodValue)variable;
                if (method.ContextIsolation.Matches(Constants.TT.KEYWORD,
                        new TokenValue(typeof(string), Constants.Keyword.PRIVATE)) && !node.Tokens[0]
                        .Matches(Constants.TT.IDENTIFIER, new TokenValue(typeof(string), Constants.Keyword.THIS)))
                {
                    return res.Failure(new RuntimeError(
                        $"Cannot access private method '{method.Name}' with outer scope", context,
                        node.StartPos,
                        node.EndPos));
                }
                else
                {
                    return res.Success(method);
                }
            }
            else if (variable.GetType() == typeof(FieldValue))
            {
                FieldValue field = (FieldValue)variable;
                if (field.ContextIsolation.Matches(Constants.TT.KEYWORD,
                        new TokenValue(typeof(string), Constants.Keyword.PRIVATE)) && !node.Tokens[0]
                        .Matches(Constants.TT.IDENTIFIER, new TokenValue(typeof(string), Constants.Keyword.THIS)))
                {
                    return res.Failure(new RuntimeError(
                        $"Cannot access private field '{field.Name}' with outer scope", context,
                        node.StartPos,
                        node.EndPos));
                }
                else
                {
                    return res.Success(field.Value);
                }
            }

            return res.Success(variable);
        }

        private RuntimeResult VisitObjectAssignNode(ObjectAssignNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            List<Token> tokens = new List<Token>(node.Tokens);
            Token current = tokens[0];
            tokens.RemoveAt(0);
            Token start = current;
            List<KeyValuePair<Token, Value>> stack = new List<KeyValuePair<Token, Value>>();
            Value variable = res.Register(Visit(new VarAccessNode(current), context));
            if (res.ShouldReturn())
            {
                return res;
            }

            stack.Add(new KeyValuePair<Token, Value>(current, variable));

            while (tokens.Count != 0)
            {
                current = tokens[0];
                tokens.RemoveAt(0);

                if (variable.GetType() == typeof(ObjectValue))
                {
                    ObjectValue rr = (ObjectValue)variable;
                    if (!rr.Elements.ContainsKey(current.Value.GetAsString()) && tokens.Count != 0)
                    {
                        return res.Failure(new RuntimeError(
                            $"'{current.Value.GetAsString()}' is not defined in the object '{start.Value.GetAsString()}'",
                            context, node.StartPos, node.EndPos));
                    }

                    stack.Add(new KeyValuePair<Token, Value>(current, variable));
                    if (tokens.Count != 0)
                    {
                        variable = rr.Elements[current.Value.GetAsString()];
                    }
                }
                else if (variable.GetType() == typeof(ListValue))
                {
                    if (current.Type != Constants.TT.INT)
                    {
                        return res.Failure(new RuntimeError("Expected int", context, node.StartPos, node.EndPos));
                    }

                    ListValue rr = (ListValue)variable;

                    int index = current.Value.GetAsInt();
                    if (index < 0 || index > rr.Elements.Count - 1 && tokens.Count != 0)
                    {
                        return res.Failure(new RuntimeError("Int is out of bounds of the list", context,
                            node.StartPos, node.EndPos));
                    }

                    stack.Add(new KeyValuePair<Token, Value>(current, variable));
                    if (tokens.Count != 0)
                    {
                        variable = rr.Elements[index];
                    }
                }

                if (tokens.Count != 0 && variable.GetType() != typeof(ObjectValue) &&
                    variable.GetType() != typeof(ListValue))
                {
                    return res.Failure(new RuntimeError($"Cannot access on '{variable.__repr__(0)}'", context,
                        node.StartPos, node.EndPos));
                }
            }


            variable = res.Register(Visit(node.Value, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            KeyValuePair<Token, Value> startPart = stack[0];
            stack.RemoveAt(0);

            while (stack.Count != 0)
            {
                Value replace = default;

                KeyValuePair<Token, Value> part = stack[0];
                stack.RemoveAt(0);
                if (part.Value.GetType() == typeof(ObjectValue))
                {
                    ObjectValue rr = (ObjectValue)part.Value;
                    rr.Elements[part.Key.Value.GetAsString()] = variable;
                    replace = rr;
                }
                else if (part.Value.GetType() == typeof(ListValue))
                {
                    ListValue rr = (ListValue)part.Value;
                    rr.Elements[part.Key.Value.GetAsInt()] = variable;
                    replace = rr;
                }

                variable = replace;
            }

            if (context.SymbolTable.IsConstants(startPart.Key.Value.GetAsString()))
            {
                return res.Failure(new RuntimeError("Cannot re-declare constants", context, node.StartPos,
                    node.EndPos));
            }

            context.SymbolTable.Set(startPart.Key.Value.GetAsString(), variable);
            return res.Success(NumberValue.Null);
        }

        private RuntimeResult VisitReturnNode(ReturnNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            Value value;

            if (node.Node != default(Node))
            {
                value = res.Register(Visit(node.Node, context));
                if (res.ShouldReturn())
                {
                    return res;
                }
            }
            else
            {
                value = NumberValue.Null;
            }

            return res.Success(value);
        }

        private RuntimeResult VisitContinueNode(ContinueNode node, Context context)
        {
            return new RuntimeResult().SuccessContinue();
        }

        private RuntimeResult VisitBreakNode(BreakNode node, Context context)
        {
            return new RuntimeResult().SuccessBreak();
        }

        private RuntimeResult VisitImportNode(ImportNode node, Context context)
        {
            string value = node.Module.Value.GetAsString();
            if (value.StartsWith("."))
            {
                if (!Constants.Config.FileImport)
                {
                    return new RuntimeResult().Failure(new RuntimeError(
                        "File import is disabled in ils.ini", context, node.StartPos, node.EndPos));
                }

                value = Path.Join(node.StartPos.Filepath, value);
                try
                {
                    string data;
                    if (MemoryCash.Exists(value, MemoryCash.FILE))
                    {
                        data = MemoryCash.Get(value).Content;
                    }
                    else
                    {
                        data = File.ReadAllText(value);
                        MemoryCash.Set(value, data);
                    }

                    FileInfo fileInfo = new FileInfo(value);
                    SymbolTable copySymbolTable = SymbolTable.GlobalSymbols;
                    SymbolTable.GlobalSymbols = new SymbolTable();
                    Context importContext = new Context("<@import>")
                    {
                        SymbolTable = SymbolTable.GlobalSymbols
                    };

                    Tuple<Error, Value, Dictionary<string, Value>> res = Executor.Run(data, fileInfo.Name,
                        fileInfo.DirectoryName, importContext, true);
                    if (res.Item1 != default(Error))
                    {
                        return new RuntimeResult().Failure(res.Item1);
                    }

                    if (res.Item3 == default(Dictionary<string, Value>))
                    {
                        return new RuntimeResult().Success(NumberValue.Null);
                    }

                    SymbolTable.GlobalSymbols = copySymbolTable;
                    foreach (KeyValuePair<string, Value> functionValue in res.Item3)
                    {
                        SymbolTable.GlobalSymbols.Set(functionValue.Key, functionValue.Value);
                    }

                    return new RuntimeResult().Success(NumberValue.Null);
                }
                catch (IOException err)
                {
                    return new RuntimeResult().Failure(new RuntimeError(
                        $"Failed to load script {value}\n\n{err.Message}", context, node.StartPos, node.EndPos));
                }
            }
            else
            {
                if (PluginLoader.Exists(value))
                {
                    PluginLoader.Bind(value, SymbolTable.GlobalSymbols);
                }
                else if (MemoryCash.Exists(value, MemoryCash.AST))
                {
                    SymbolTable copySymbolTable = SymbolTable.GlobalSymbols;
                    SymbolTable.GlobalSymbols = new SymbolTable();
                    Context importContext = new Context("<@import>")
                    {
                        SymbolTable = SymbolTable.GlobalSymbols
                    };
                    Tuple<Error, Value, Dictionary<string, Value>> res =
                        Executor.RunAst((ListNode)MemoryCash.Get(value).Node, importContext, false);
                    if (res.Item1 != default(Error))
                    {
                        return new RuntimeResult().Failure(res.Item1);
                    }

                    if (res.Item3 == default(Dictionary<string, Value>))
                    {
                        return new RuntimeResult().Success(NumberValue.Null);
                    }

                    SymbolTable.GlobalSymbols = copySymbolTable;
                    foreach (KeyValuePair<string, Value> functionValue in res.Item3)
                    {
                        SymbolTable.GlobalSymbols.Set(functionValue.Key, functionValue.Value);
                    }

                    return new RuntimeResult().Success(NumberValue.Null);
                }
                else
                {
                    return new RuntimeResult().Failure(new RuntimeError(
                        $"'{value}' is not a file or module or exists in a bundle", context,
                        node.StartPos, node.EndPos));
                }
            }

            return new RuntimeResult().Success(NumberValue.Null);
        }

        private RuntimeResult VisitUseNode(UseNode node, Context context)
        {
            string value = node.Module.Value.GetAsString();

            if (value.StartsWith("."))
            {
                value = Path.Join(node.StartPos.Filepath, value);
                try
                {
                    List<BundleEntry> files = Converter.ReadBundle(value);
                    foreach (BundleEntry file in files)
                    {
                        Node ast = Node.ConvertNode(file.RawNode);
                        MemoryCash.Set(file.AccessName, ast);
                    }

                    return new RuntimeResult().Success(NumberValue.Null);
                }
                catch (IOException err)
                {
                    return new RuntimeResult().Failure(new RuntimeError(
                        $"Failed to load bundle {Extensions.Path.Join(value)}\n\n{err.Message}", context, node.StartPos,
                        node.EndPos));
                }
            }
            else
            {
                return new RuntimeResult().Failure(new RuntimeError("Path must begin with a dot", context,
                    node.StartPos, node.EndPos));
            }
        }

        private RuntimeResult VisitPackageNode(PackageNode node, Context context)
        {
            return new RuntimeResult().Success(NumberValue.Null);
        }

        private RuntimeResult VisitHeadIfNode(HeadIfNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            foreach (IfCase nodeCase in node.Cases)
            {
                bool shouldReturnNull = nodeCase.Bool;
                Value conditionValue = res.Register(Visit(nodeCase.Condition, context));
                if (res.ShouldReturn())
                {
                    return res;
                }

                if (conditionValue.IsTrue())
                {
                    Value exprValue = res.Register(Visit(nodeCase.Statements, context));
                    if (res.ShouldReturn())
                    {
                        return res;
                    }

                    return res.Success((shouldReturnNull) ? NumberValue.Null : exprValue);
                }
            }

            if (node.ElseCase != default(ElseCaseNode))
            {
                Value elseValue = res.Register(Visit(node.ElseCase.Statements, context));
                if (res.ShouldReturn())
                {
                    return res;
                }

                return res.Success((node.ElseCase.Bool) ? NumberValue.Null : elseValue);
            }

            return res.Success(NumberValue.Null);
        }

        private RuntimeResult VisitExportNode(ExportNode node, Context context)
        {
            if (!Constants.Config.FileExport)
            {
                return new RuntimeResult().Failure(new RuntimeError(
                    "Export is disabled in ils.ini", context, node.StartPos, node.EndPos));
            }

            if (node.Func.GetType() == typeof(FunctionDefineNode))
            {
                FunctionDefineNode func = (FunctionDefineNode)node.Func;
                if (func.VarName == default(Token))
                {
                    return new RuntimeResult().Failure(new RuntimeError(
                        "Cannot export anonymous function", context, node.StartPos, node.EndPos));
                }
            }

            RuntimeResult res = Visit(node.Func, context);
            if (res.ShouldReturn())
            {
                return res;
            }

            string exportName = node.Name;
            Exports[exportName] = res.Value;
            return new RuntimeResult().Success(NumberValue.Null);
        }

        private RuntimeResult VisitMainNode(MainNode node, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            SymbolTable symbolTable = context.SymbolTable;
            if (symbolTable.HasInCurrent("main"))
            {
                FunctionValue func = (FunctionValue)symbolTable.Get("main").Value;
                if (func.ArgNames.Count == 2)
                {
                    if (func.ArgNames[0] == "argv" && func.ArgNames[1] == "argc")
                    {
                        List<Node> nodes = new List<Node>();
                        foreach (string s in Argv)
                        {
                            nodes.Add(new StringNode(new Token(Constants.TT.STRING,
                                new TokenValue(typeof(string), s))));
                        }

                        List<Node> sendArgs = new List<Node>();
                        sendArgs.Add(new ListNode(nodes, Position.Empty(), Position.Empty()));
                        sendArgs.Add(new NumberNode(new Token(Constants.TT.INT,
                            new TokenValue(typeof(int), Argv.Count.ToString()))));

                        res.Register(Visit(new CallNode(
                            new VarAccessNode(
                                new Token(
                                    Constants.TT.STRING,
                                    new TokenValue(typeof(string), "main")
                                )
                            ),
                            sendArgs
                        ), context));
                        if (res.ShouldReturn())
                        {
                            return res;
                        }
                    }
                }
            }

            return res.Success(NumberValue.Null);
        }
    }
}
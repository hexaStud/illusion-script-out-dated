using System.Collections.Generic;
using IllusionScript.SDK.Errors;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class MethodValue : ClassItemValue
    {
        public readonly Node BodyNode;
        public readonly List<string> ArgNames;
        public readonly bool ShouldReturnAuto;

        public MethodValue(Token contextIsolation, string name, Node bodyNode, List<string> argNames,
            bool shouldReturnAuto) : base(contextIsolation, name, default)
        {
            BodyNode = bodyNode;
            ArgNames = argNames;
            ShouldReturnAuto = shouldReturnAuto;
            Self = this;
        }

        public override RuntimeResult Execute(List<Value> args, Value self = default)
        {
            RuntimeResult res = new RuntimeResult();
            Interpreter interpreter = new Interpreter();
            Context context = GenerateNewContext();

            res.Register(CheckAndPopulate(ArgNames, args, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            if (self == default(Value))
            {
                return res.Failure(new RuntimeError("'this' must be declared by a method call", context, StartPos,
                    EndPos));
            }

            context.SymbolTable.Set("this", self);
            Value value = res.Register(interpreter.Visit(BodyNode, context));
            if (res.ShouldReturn() && res.FunctionReturn == default(Value))
            {
                return res;
            }

            Value returnValue = (ShouldReturnAuto) ? value : NumberValue.Null;
            return res.Success(returnValue);
        }

        public override Value Copy()
        {
            MethodValue copy = new MethodValue(ContextIsolation, Name, BodyNode, ArgNames, ShouldReturnAuto);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override string __repr__(int stage)
        {
            return $"<method [{ContextIsolation.Value.GetAsString()}] {Name}[{StartPos.FileName} ({StartPos.Ln + 1})]>";
        }
    }
}
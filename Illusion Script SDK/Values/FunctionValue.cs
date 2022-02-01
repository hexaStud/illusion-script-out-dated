using System.Collections.Generic;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class FunctionValue : BaseFunctionValue
    {
        public List<string> ArgNames;
        public Node BodyNode;
        public bool ShouldReturnAuto;

        public FunctionValue(string name, Node bodyNode, List<string> argNames, bool shouldReturnAuto) : base(name)
        {
            BodyNode = bodyNode;
            ArgNames = argNames;
            ShouldReturnAuto = shouldReturnAuto;
        }

        public override RuntimeResult Execute(List<Value> args, Value self = default)
        {
            var res = new RuntimeResult();
            var interpreter = new Interpreter();
            var context = GenerateNewContext();

            res.Register(CheckAndPopulate(ArgNames, args, context));
            if (res.ShouldReturn()) return res;

            if (self != default(Value)) context.SymbolTable.Set("this", self);

            var value = res.Register(interpreter.Visit(BodyNode, context));
            if (res.ShouldReturn() && res.FunctionReturn == default(Value)) return res;

            var returnValue = ShouldReturnAuto ? value : NumberValue.Null;
            return res.Success(returnValue);
        }

        public override Value Copy()
        {
            var copy = new FunctionValue(Name, BodyNode, ArgNames, ShouldReturnAuto);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override string __repr__(int stage)
        {
            return $"<function {Name}[{StartPos.FileName} ({StartPos.Ln + 1})]>";
        }
    }
}
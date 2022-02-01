using System;
using System.Collections.Generic;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class BuiltInFunctionValue : BaseFunctionValue
    {
        private static readonly Dictionary<string, IBuiltInFunction> BaseInFunctions = new();

        private BuiltInFunctionValue(string name) : base(name)
        {
        }

        public override RuntimeResult Execute(List<Value> args, Value self = default)
        {
            var res = new RuntimeResult();
            var context = GenerateNewContext();

            if (!BaseInFunctions.ContainsKey(Name)) throw NoVisitMethod(Name);

            var func = BaseInFunctions[Name];
            res.Register(CheckAndPopulate(func.Args, args, context));
            if (res.ShouldReturn()) return res;

            var returnValue = res.Register(func.Exec(context, this));
            if (res.ShouldReturn()) return res;

            return res.Success(returnValue);
        }

        public Exception NoVisitMethod(string name)
        {
            throw new Exception($"No {name} method defined");
        }

        public override Value Copy()
        {
            var copy = new BuiltInFunctionValue(Name);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override string __repr__(int stage)
        {
            return $"<function {Name}[Native]>";
        }

        public override bool IsBuiltIn()
        {
            return true;
        }

        public static BuiltInFunctionValue Define(string name, IBuiltInFunction func)
        {
            BaseInFunctions[name] = func;
            return new BuiltInFunctionValue(name);
        }
    }
}
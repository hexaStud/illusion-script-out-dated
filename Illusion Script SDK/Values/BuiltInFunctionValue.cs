using System;
using System.Collections.Generic;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class BuiltInFunctionValue : BaseFunctionValue
    {
        public static Dictionary<string, IBuiltInFunction> BaseInFunctions = new Dictionary<string, IBuiltInFunction>();

        public BuiltInFunctionValue(string name) : base(name)
        {
        }

        public override RuntimeResult Execute(List<Value> args, Value self = default)
        {
            RuntimeResult res = new RuntimeResult();
            Context context = GenerateNewContext();

            if (!BaseInFunctions.ContainsKey(Name))
            {
                throw NoVisitMethod(Name);
            }

            IBuiltInFunction func = BaseInFunctions[Name];
            res.Register(CheckAndPopulate(func.Args, args, context));
            if (res.ShouldReturn())
            {
                return res;
            }

            Value returnValue = res.Register(func.Exec(context, this));
            if (res.ShouldReturn())
            {
                return res;
            }

            return res.Success(returnValue);
        }

        public Exception NoVisitMethod(string name)
        {
            throw new Exception($"No {name} method defined");
        }

        public override Value Copy()
        {
            BuiltInFunctionValue copy = new BuiltInFunctionValue(Name);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override string __repr__(int stage)
        {
            return $"<function {Name}[Native]>";
        }

        public static BuiltInFunctionValue Define(string name, IBuiltInFunction func)
        {
            BaseInFunctions[name] = func;
            return new BuiltInFunctionValue(name);
        }
    }
}
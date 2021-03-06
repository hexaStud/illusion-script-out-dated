using System;
using System.Collections.Generic;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class BuildInFunctionValue : BaseFunctionValue
    {
        private static readonly Dictionary<string, IBuildInFunction> BaseInFunctions =
            new Dictionary<string, IBuildInFunction>();

        private BuildInFunctionValue(string name) : base(name)
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

            IBuildInFunction func = BaseInFunctions[Name];
            res.Register(CheckAndPopulate(func.Args, args, context));
            if (res.ShouldReturn())
            {
                return res;
            }
            
            if (self != default(Value))
            {
                context.SymbolTable.Set("this", self);
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
            BuildInFunctionValue copy = new BuildInFunctionValue(Name);
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

        public static BuildInFunctionValue Define(string name, IBuildInFunction func)
        {
            BaseInFunctions[name] = func;
            return new BuildInFunctionValue(name);
        }
    }
}
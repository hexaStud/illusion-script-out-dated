using System.Collections.Generic;
using IllusionScript.SDK.Errors;
using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Values
{
    public class BuildInMethodValue : ClassItemValue
    {
        private IBuildInMethod Method;

        public BuildInMethodValue(IBuildInMethod method) : base(method.Isolation, method.Name, default)
        {
            Self = this;
            Method = method;
        }

        public override RuntimeResult Execute(List<Value> args, Value self = default)
        {
            RuntimeResult res = new RuntimeResult();
            Context context = GenerateNewContext();

            res.Register(CheckAndPopulate(Method.ArgNames, args, context));
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
            Value returnValue = res.Register(Method.Exec(args, this));
            if (res.ShouldReturn())
            {
                return res;
            }

            return res.Success(returnValue);
        }

        public override Value Copy()
        {
            BuildInMethodValue copy = new BuildInMethodValue(Method);
            copy.SetContext(Context);
            copy.SetPosition(StartPos, EndPos);
            return copy;
        }

        public override bool IsBuiltIn()
        {
            return true;
        }

        public override string __repr__(int stage)
        {
            return $"<method [{ContextIsolation.Value.GetAsString()}] {Name}[Native]>";
        }
    }
}
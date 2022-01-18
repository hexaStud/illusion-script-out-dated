using System.Collections.Generic;
using IllusionScript.SDK.Errors;

namespace IllusionScript.SDK.Values.Assets
{
    public abstract class ClassItemValue : Value
    {
        public Token ContextIsolation;
        public string Name;
        public Value Self;

        protected ClassItemValue(Token contextIsolation, string name, Value self)
        {
            ContextIsolation = contextIsolation;
            Name = name;
            Self = self;
        }

        protected Context GenerateNewContext()
        {
            Context context = new Context(Name, Context, StartPos);
            context.SymbolTable = new SymbolTable(Context.SymbolTable);
            return context;
        }

        protected RuntimeResult CheckArgs(List<string> argsName, List<Value> args)
        {
            RuntimeResult res = new RuntimeResult();
            if (args.Count < argsName.Count)
            {
                return res.Failure(new RuntimeError(
                    $"{argsName.Count - args.Count} too many args passed into '{Name}'", Context, StartPos, EndPos));
            }

            if (args.Count > argsName.Count)
            {
                return res.Failure(new RuntimeError(
                    $"{argsName.Count - args.Count} too many args passed into '{Name}'", Context, StartPos, EndPos));
            }

            return res.Success(NumberValue.Null);
        }

        protected void PopulateArgs(List<string> argNames, List<Value> args, Context context)
        {
            for (int i = 0; i < args.Count; i++)
            {
                string name = argNames[i];
                Value value = args[i];

                value.SetContext(context);
                context.SymbolTable.Set(name, value);
            }
        }

        protected RuntimeResult CheckAndPopulate(List<string> argNames, List<Value> args, Context context)
        {
            RuntimeResult res = new RuntimeResult();
            res.Register(CheckArgs(argNames, args));
            if (res.ShouldReturn())
            {
                return res;
            }

            PopulateArgs(argNames, args, context);
            return res.Success(NumberValue.Null);
        }
    }
}
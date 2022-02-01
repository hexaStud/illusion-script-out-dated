﻿using System.Collections.Generic;
using IllusionScript.SDK.Errors;

namespace IllusionScript.SDK.Values.Assets
{
    public abstract class ClassItemValue : Value
    {
        public static readonly Token PUBLIC =
            new(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.PUBLIC));

        public static readonly Token PRIVATE =
            new(Constants.TT.KEYWORD, new TokenValue(typeof(string), Constants.Keyword.PRIVATE));

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
            var context = new Context(Name, Context, StartPos)
            {
                SymbolTable = new SymbolTable(Context.SymbolTable)
            };
            return context;
        }

        protected RuntimeResult CheckArgs(List<string> argsName, List<Value> args)
        {
            var res = new RuntimeResult();
            if (args.Count < argsName.Count)
                return res.Failure(new RuntimeError(
                    $"{argsName.Count - args.Count} too many args passed into '{Name}'", Context, StartPos, EndPos));

            if (args.Count > argsName.Count)
                return res.Failure(new RuntimeError(
                    $"{argsName.Count - args.Count} too many args passed into '{Name}'", Context, StartPos, EndPos));

            return res.Success(NumberValue.Null);
        }

        protected void PopulateArgs(List<string> argNames, List<Value> args, Context context)
        {
            for (var i = 0; i < args.Count; i++)
            {
                var name = argNames[i];
                var value = args[i];

                value.SetContext(context);
                context.SymbolTable.Set(name, value);
            }
        }

        protected RuntimeResult CheckAndPopulate(List<string> argNames, List<Value> args, Context context)
        {
            var res = new RuntimeResult();
            res.Register(CheckArgs(argNames, args));
            if (res.ShouldReturn()) return res;

            PopulateArgs(argNames, args, context);
            return res.Success(NumberValue.Null);
        }
    }
}
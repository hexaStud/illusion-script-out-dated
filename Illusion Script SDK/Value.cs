using System;
using System.Collections.Generic;
using IllusionScript.SDK.Errors;
using IllusionScript.SDK.Values;

namespace IllusionScript.SDK
{
    public class Value
    {
        public Position StartPos;
        public Position EndPos;
        protected Context Context;

        public Value SetPosition(Position startPos, Position endPos)
        {
            StartPos = startPos;
            EndPos = endPos;
            return this;
        }

        public Value SetContext(Context context)
        {
            Context = context;
            return this;
        }

        public virtual Tuple<Error, Value> AddedTo(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> SubbedBy(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> MultedBy(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> DivedBy(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> ModuloedBy(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> PowedBy(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> GetComparisonEQ(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> GetComparisonNEQ(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> GetComparisonLT(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> GetComparisonGT(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> GetComparisonLEQ(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> GetComparisonGEQ(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> AndedBy(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> OredBy(Value other)
        {
            return new Tuple<Error, Value>(IllegalOperation(other), default);
        }

        public virtual Tuple<Error, Value> Notted()
        {
            return new Tuple<Error, Value>(IllegalOperation(), default);
        }

        public virtual RuntimeResult Execute(List<Value> args, Value self = default)
        {
            return new RuntimeResult().Failure(IllegalOperation());
        }

        public virtual RuntimeResult Construct(List<Value> args)
        {
            return new RuntimeResult().Failure(IllegalOperation());
        }

        public virtual Value ObjectAccess()
        {
            ObjectValue value = new ObjectValue(new Dictionary<string, Value>());
            value.SetContext(Context).SetPosition(StartPos, EndPos);
            return value;
        }

        public virtual bool IsTrue()
        {
            return false;
        }

        public virtual Value Copy()
        {
            throw new Exception("No copy method defend");
        }

        protected Error IllegalOperation()
        {
            return IllegalOperation(this);
        }

        protected Error IllegalOperation(Value other)
        {
            return new RuntimeError("Illegal operation", Context, StartPos, EndPos);
        }

        public virtual string __repr__(int stage)
        {
            return "";
        }

        public virtual bool IsBuiltIn()
        {
            return false;
        }
    }
}
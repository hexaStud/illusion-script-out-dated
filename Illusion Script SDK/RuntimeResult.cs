namespace IllusionScript.SDK
{
    public class RuntimeResult
    {
        public Value Value;
        public Error Error;
        public Value FunctionReturn;
        public bool LoopShouldContinue;
        public bool LoopShouldBreak;

        public RuntimeResult()
        {
            LoopShouldContinue = false;
            LoopShouldBreak = false;
            Reset();
        }

        public void Reset()
        {
            Value = default;
            Error = default;
            FunctionReturn = default;
            LoopShouldContinue = false;
            LoopShouldBreak = false;
        }

        public Value Register(RuntimeResult res)
        {
            Error = res.Error;
            FunctionReturn = res.FunctionReturn;
            LoopShouldContinue = res.LoopShouldContinue;
            LoopShouldBreak = res.LoopShouldBreak;
            return res.Value;
        }

        public RuntimeResult Success(Value value)
        {
            Reset();
            Value = value;
            return this;
        }

        public RuntimeResult SuccessContinue()
        {
            Reset();
            LoopShouldContinue = true;
            return this;
        }

        public RuntimeResult SuccessBreak()
        {
            Reset();
            LoopShouldBreak = true;
            return this;
        }

        public RuntimeResult Failure(Error error)
        {
            Reset();
            Error = error;
            return this;
        }

        public bool ShouldReturn()
        {
            return Error != default(Error) || FunctionReturn != default(Value) || LoopShouldContinue != default(bool) ||
                   LoopShouldBreak != default(bool);
        }
    }
}
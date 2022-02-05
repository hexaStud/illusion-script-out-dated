using System.Collections.Generic;

namespace IllusionScript.SDK.Values.Assets
{
    public interface IBuildInMethod : IBuildInClassItem
    {
        public List<string> ArgNames { get; }
        public RuntimeResult Exec(List<Value> args, BuildInMethodValue self);
    }
}
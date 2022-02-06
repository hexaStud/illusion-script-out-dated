using System.Collections.Generic;

namespace IllusionScript.SDK.Values.Assets
{
    public interface IBuildInFunction
    {
        List<string> Args { get; }

        RuntimeResult Exec(Context context, BuildInFunctionValue self);
    }
}
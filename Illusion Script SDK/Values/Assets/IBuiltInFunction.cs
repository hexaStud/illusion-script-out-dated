using System.Collections.Generic;

namespace IllusionScript.SDK.Values.Assets
{
    public interface IBuiltInFunction
    {
        public static string Name { get; }
        List<string> Args { get; }

        RuntimeResult Exec(Context context, BuiltInFunctionValue self);
    }
}
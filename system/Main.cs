using IllusionScript.SDK;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;

namespace IllusionScript.Lib.system
{
    public class Main : IModule
    {
        public string Name { get; } = "system";

        public void Load(SymbolTable table)
        {
            table.Set(Application.ClassName, BuildInClassValue.Define(Application.ClassName, new Application()));
        }
    }
}
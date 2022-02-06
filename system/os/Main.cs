using System.Runtime.InteropServices;
using IllusionScript.SDK;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;

namespace IllusionScript.Lib.system.os
{
    public class Main : IModule
    {
        public string Name { get; } = "os";

        public void Load(SymbolTable table)
        {
            // ===== CONSTANCE ===== //
            table.Set("OS_NAME", new StringValue(new TokenValue(typeof(string), GetOsName())), true);
            table.Set("OS_ARCH",
                new StringValue(new TokenValue(typeof(string), RuntimeInformation.OSArchitecture.ToString())));
        }

        private string GetOsName()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" :
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" :
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "OSX" :
                RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ? "Free BSD" :
                "Unknown";
        }
    }
}
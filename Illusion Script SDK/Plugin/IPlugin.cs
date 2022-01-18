using IllusionScript.SDK.Values.Assets;

namespace IllusionScript.SDK.Plugin
{
    public interface IPlugin : IBuiltInFunction
    {
        public static string Name { get; }
    }
}
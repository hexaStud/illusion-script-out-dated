namespace IllusionScript.SDK.Plugin
{
    public interface IModule
    {
        public string Name { get; }
        void Load(SymbolTable table);
    }
}
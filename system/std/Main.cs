using IllusionScript.SDK;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;

namespace IllusionScript.Lib.system.std
{
    public class Main : IModule
    {
        public string Name { get; } = "std";

        public void Load(SymbolTable table)
        {
            // ===== CONSTANCE ===== //
            table.Set("null", NumberValue.Null, true);
            table.Set("false", NumberValue.False, true);
            table.Set("true", NumberValue.True, true);
            table.Set("endl", new StringValue(new TokenValue(typeof(string), Constants.EOL)));

            // ===== CONSOLE FUNCTIONS ===== //
            table.Set(Print.Name, BuildInFunctionValue.Define(Print.Name, new Print()));
            table.Set(PrintLn.Name, BuildInFunctionValue.Define(PrintLn.Name, new PrintLn()));
            table.Set(Read.Name, BuildInFunctionValue.Define(Read.Name, new Read()));

            // ===== DATATYPE FUNCTIONS ===== //
            table.Set(IsInt.Name, BuildInFunctionValue.Define(IsInt.Name, new IsInt()));
            table.Set(IsFloat.Name, BuildInFunctionValue.Define(IsFloat.Name, new IsFloat()));
            table.Set(IsString.Name, BuildInFunctionValue.Define(IsString.Name, new IsString()));
            table.Set(IsList.Name, BuildInFunctionValue.Define(IsList.Name, new IsList()));
            table.Set(IsObject.Name, BuildInFunctionValue.Define(IsObject.Name, new IsObject()));
            table.Set(std.ToString.Name, BuildInFunctionValue.Define(std.ToString.Name, new std.ToString()));

            // ===== UTILS FUNCTIONS ===== //
            table.Set(Count.Name, BuildInFunctionValue.Define(Count.Name, new Count()));
        }
    }
}
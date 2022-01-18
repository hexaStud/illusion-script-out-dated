using IllusionScript.SDK;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;

namespace IllusionScript.Lib.std
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
            table.Set(Print.Name, BuiltInFunctionValue.Define(Print.Name, new Print()));
            table.Set(Read.Name, BuiltInFunctionValue.Define(Read.Name, new Read()));

            // ===== DATATYPE FUNCTIONS ===== //
            table.Set(IsInt.Name, BuiltInFunctionValue.Define(IsInt.Name, new IsInt()));
            table.Set(IsFloat.Name, BuiltInFunctionValue.Define(IsFloat.Name, new IsFloat()));
            table.Set(IsString.Name, BuiltInFunctionValue.Define(IsString.Name, new IsString()));
            table.Set(IsList.Name, BuiltInFunctionValue.Define(IsList.Name, new IsList()));
            table.Set(IsObject.Name, BuiltInFunctionValue.Define(IsObject.Name, new IsObject()));
            table.Set(std.ToString.Name, BuiltInFunctionValue.Define(std.ToString.Name, new std.ToString()));

            // ===== UTILS FUNCTIONS ===== //
            table.Set(Count.Name, BuiltInFunctionValue.Define(Count.Name, new Count()));
        }
    }
}
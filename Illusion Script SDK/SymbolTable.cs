using System.Collections.Generic;

namespace IllusionScript.SDK
{
    public class SymbolTable
    {
        public static readonly SymbolTable GlobalSymbols = new SymbolTable();
        private readonly Dictionary<string, SymbolTableValue> Symbols;
        private readonly SymbolTable Parent;

        public SymbolTable()
        {
            Symbols = new Dictionary<string, SymbolTableValue>();
        }

        public SymbolTable(SymbolTable parent)
        {
            Symbols = new Dictionary<string, SymbolTableValue>();
            Parent = parent;
        }

        public SymbolTableValue Get(string name)
        {
            SymbolTableValue value = default;
            if (Symbols.ContainsKey(name))
            {
                value = Symbols[name];
            }
            else if (Parent != default(SymbolTable))
            {
                value = Parent.Get(name);
            }

            return value;
        }

        public void Set(string name, Value value, bool constants = false)
        {
            Symbols[name] = new SymbolTableValue()
            {
                Constants = constants,
                Value = value
            };
        }

        public void Update(string name, Value value, bool constants = false)
        {
            if (HasInCurrent(name))
            {
                Symbols[name] = new SymbolTableValue()
                {
                    Value = value,
                    Constants = constants
                };
            }
            else
            {
                Parent.Update(name, value, constants);
            }
        }

        public bool HasInCurrent(string name)
        {
            return Symbols.ContainsKey(name);
        }

        public bool HasInAll(string name)
        {
            bool value = false;
            if (Symbols.ContainsKey(name))
            {
                value = true;
            }
            else if (Parent != default(SymbolTable))
            {
                value = Parent.HasInAll(name);
            }

            return value;
        }

        public bool IsConstants(string name)
        {
            if (HasInCurrent(name))
            {
                SymbolTableValue value = Get(name);
                return value.Constants;
            }

            return false;
        }

        public void Remove(string name)
        {
            if (HasInCurrent(name))
            {
                Symbols.Remove(name);
            }
        }

        public Dictionary<string, SymbolTableValue>.KeyCollection GetKeys()
        {
            return Symbols.Keys;
        }
    }
}
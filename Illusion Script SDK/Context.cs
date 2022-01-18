namespace IllusionScript.SDK
{
    public class Context
    {
        public string DisplayName;
        public Context Parent;
        public Position ParentEntryPos;
        public SymbolTable SymbolTable;

        public Context(string displayName)
        {
            DisplayName = displayName;
        }

        public Context(string displayName, Context parent, Position parentEntryPos)
        {
            DisplayName = displayName;
            Parent = parent;
            ParentEntryPos = parentEntryPos;
        }
    }
}
using System.Collections.Generic;
using IllusionScript.SDK.Extensions;

namespace IllusionScript.SDK
{
    public class MemoryCash
    {
        private readonly Dictionary<string, MemoryItem> Memory;

        public MemoryCash()
        {
            Memory = new Dictionary<string, MemoryItem>();
        }

        public void Set(string path, string content)
        {
            path = Path.Join(path);
            Memory[path] = new MemoryItem
            {
                Content = content,
                Node = default
            };
        }

        public void Set(string path, Node node)
        {
            path = Path.Join(path);
            Memory[path] = new MemoryItem
            {
                Content = default,
                Node = node
            };
        }

        public bool Exists(string path)
        {
            return Memory.ContainsKey(Path.Join(path));
        }

        public MemoryItem Get(string path)
        {
            return Memory[Path.Join(path)];
        }
    }
}
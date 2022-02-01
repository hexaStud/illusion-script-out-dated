using System.Collections.Generic;

namespace IllusionScript.SDK.Extensions
{
    public class Path
    {
        public static string Join(params string[] args)
        {
            var parts = new List<string>();
            foreach (var arg in args) parts.AddRange(arg.Replace("/", "\\").Split("\\"));

            var path = new List<string>();

            for (var i = 0; i < parts.Count; i++)
            {
                var part = parts[i];
                if (part == "") continue;

                if (part == "..")
                {
                    if (path.Count != 0) path.RemoveAt(path.Count - 1);
                }
                else if (part != ".")
                {
                    path.Add(part);
                }
            }

            var str = "";
            for (var i = 0; i < path.Count; i++)
            {
                str += path[i];
                if (i < path.Count - 1) str += "\\";
            }

            return str;
        }
    }
}
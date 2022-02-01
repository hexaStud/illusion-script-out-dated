using System.Collections.Generic;

namespace IllusionScript.SDK.Extensions
{
    public class Path
    {
        public static string Join(params string[] args)
        {
            List<string> parts = new List<string>();
            foreach (string arg in args)
            {
                parts.AddRange(arg.Replace("/", "\\").Split("\\"));
            }

            List<string> path = new List<string>();

            for (int i = 0; i < parts.Count; i++)
            {
                string part = parts[i];
                if (part == "")
                {
                    continue;
                }

                if (part == "..")
                {
                    if (path.Count != 0)
                    {
                        path.RemoveAt(path.Count -1);
                    }
                }
                else if (part != ".")
                {
                    path.Add(part);
                }
            }

            string str = "";
            for (int i = 0; i < path.Count; i++)
            {
                str += path[i];
                if (i < path.Count - 1)
                {
                    str += "\\";
                }
            }

            return str;
        }
    }
}
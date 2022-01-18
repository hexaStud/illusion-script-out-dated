using System.Collections.Generic;
using System.IO;

namespace IllusionScript
{
    public class InIFile
    {
        private readonly string Path;
        private readonly string[] Lines;

        public InIFile(string path)
        {
            Path = path;
            string[] lines = File.ReadAllLines(Path);
            List<string> list = new List<string>();
            foreach (string line in lines)
            {
                if (!line.StartsWith(";"))
                {
                    list.Add(line);
                }
            }

            Lines = list.ToArray();
        }

        public List<string> Read(string section, string key)
        {
            List<string> res = new List<string>();
            string currentSection = "";

            foreach (string readerLine in Lines)
            {
                if (readerLine.StartsWith("[") && readerLine.EndsWith("]"))
                {
                    currentSection = readerLine;
                }
                else if (currentSection == $"[{section}]")
                {
                    string[] lineParts = readerLine.Split("=", 2);

                    if (lineParts.Length >= 1 && lineParts[0].TrimStart().TrimEnd() == key)
                    {
                        res.Add(lineParts[1].TrimStart().TrimEnd());
                    }
                }
            }

            return res;
        }
    }
}
using System.Collections.Generic;
using System.IO;

namespace IllusionScript
{
    public class InIFile
    {
        private readonly string[] Lines;

        public InIFile(string path)
        {
            var lines = File.ReadAllLines(path);
            var list = new List<string>();
            foreach (var line in lines)
                if (!line.StartsWith(";"))
                    list.Add(line);

            Lines = list.ToArray();
        }

        public List<string> Read(string section, string key)
        {
            var res = new List<string>();
            var currentSection = "";

            foreach (var readerLine in Lines)
                if (readerLine.StartsWith("[") && readerLine.EndsWith("]"))
                {
                    currentSection = readerLine;
                }
                else if (currentSection == $"[{section}]")
                {
                    var lineParts = readerLine.Split("=", 2);

                    if (lineParts.Length >= 1 && lineParts[0].TrimStart().TrimEnd() == key)
                        res.Add(lineParts[1].TrimStart().TrimEnd());
                }

            return res;
        }
    }
}
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace IllusionScript.SDK.Bundler
{
    public class HashMap
    {
        private Dictionary<string, string> Data;

        private HashMap(Dictionary<string, string> entries)
        {
            Data = entries;
        }

        public Dictionary<string, string>.KeyCollection Keys()
        {
            return Data.Keys;
        }

        public string this[string index] => Data[index];

        public static HashMap GetHashMap(ZipArchive arch)
        {
            ZipArchiveEntry entry = arch.GetEntry("hashmap");
            if (entry != null)
            {
                Stream stream = entry.Open();
                StreamReader reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
                string[] lines = content.Split("\n");
                Dictionary<string, string> map = new Dictionary<string, string>();

                foreach (string line in lines)
                {
                    string[] parts = line.Split("=");
                    if (parts.Length == 2)
                    {
                        map[parts[0]] = parts[1];
                    }
                }

                return new HashMap(map);
            }
            else
            {
                throw new Exception("Cannot find hashmap in ila");
            }
        }
    }
}
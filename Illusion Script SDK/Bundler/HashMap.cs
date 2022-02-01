using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace IllusionScript.SDK.Bundler
{
    public class HashMap
    {
        private readonly Dictionary<string, string> Data;

        private HashMap(Dictionary<string, string> entries)
        {
            Data = entries;
        }

        public string this[string index] => Data[index];

        public Dictionary<string, string>.KeyCollection Keys()
        {
            return Data.Keys;
        }

        public static HashMap GetHashMap(ZipArchive arch)
        {
            var entry = arch.GetEntry("hashmap");
            if (entry != null)
            {
                var stream = entry.Open();
                var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
                var lines = content.Split("\n");
                var map = new Dictionary<string, string>();

                foreach (var line in lines)
                {
                    var parts = line.Split("=");
                    if (parts.Length == 2) map[parts[0]] = parts[1];
                }

                return new HashMap(map);
            }

            throw new Exception("Cannot find hashmap in ila");
        }
    }
}
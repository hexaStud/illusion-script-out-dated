using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using IllusionScript.SDK.Nodes;

namespace IllusionScript.SDK.Bundler
{
    public class Converter
    {
        private List<BundleFile> Bundles;

        public Converter()
        {
            Bundles = new List<BundleFile>();
        }

        public void Bundle(ListNode node)
        {
            string content = node.__bundle__();
            string hash = DateTimeOffset.Now.ToUnixTimeSeconds().ToString("X");

            Bundles.Add(new BundleFile()
            {
                Content = content,
                Hash = hash,
                AccessName = node.StartPos.FileName
            });
        }

        public void WriteDown(string path)
        {
            string tmp = Path.Join(Path.GetTempPath(), "ils-" + DateTimeOffset.Now.ToUnixTimeSeconds());
            if (Directory.Exists(tmp))
            {
                Directory.Delete(tmp, true);
            }

            Directory.CreateDirectory(tmp);
            ZipArchive zipFile = ZipFile.Open(path, ZipArchiveMode.Create);
            Dictionary<string, string> hashMap = new Dictionary<string, string>();

            foreach (BundleFile bundleFile in Bundles)
            {
                StreamWriter stream = new StreamWriter(Path.Join(tmp, bundleFile.Hash));
                stream.Write(bundleFile.Content);
                stream.Close();

                zipFile.CreateEntryFromFile(Path.Join(tmp, bundleFile.Hash), bundleFile.Hash);
                hashMap[bundleFile.AccessName] = bundleFile.Hash;
            }

            StreamWriter hashMapStream = new StreamWriter(Path.Join(tmp, "hashmap"));
            foreach (KeyValuePair<string, string> valuePair in hashMap)
            {
                hashMapStream.Write($"{valuePair.Key}={valuePair.Value}");
            }

            hashMapStream.Close();

            zipFile.CreateEntryFromFile(Path.Join(tmp, "hashmap"), "hashmap");
            zipFile.Dispose();

            Directory.Delete(tmp, true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using IllusionScript.SDK.Nodes;

namespace IllusionScript.SDK.Bundler
{
    public class Converter
    {
        private readonly List<BundleFile> Bundles;

        public Converter()
        {
            Bundles = new List<BundleFile>();
        }

        public void Bundle(ListNode node)
        {
            var content = node.__bundle__();
            var hash = (DateTimeOffset.Now.ToUnixTimeMilliseconds() / 2).ToString("X");

            PackageNode packageNode = default;

            foreach (var nodeElement in node.Elements)
                if (nodeElement.GetType() == typeof(PackageNode))
                {
                    packageNode = (PackageNode)nodeElement;
                    break;
                }

            if (packageNode == default(PackageNode))
                throw new Exception(
                    $"Missing package declaration in '{Path.Join(node.StartPos.Filepath, node.StartPos.FileName)}'");

            var accessName = "";
            foreach (var token in packageNode.Names) accessName += token.Value.GetAsString() + ".";

            accessName += packageNode.StartPos.FileName;

            Bundles.Add(new BundleFile
            {
                Content = content,
                Hash = hash,
                AccessName = accessName
            });
        }

        public void WriteDown(string path, bool overwrite)
        {
            var tmp = Path.Join(Path.GetTempPath(), "ils-" + DateTimeOffset.Now.ToUnixTimeSeconds());
            if (Directory.Exists(tmp)) Directory.Delete(tmp, true);

            Directory.CreateDirectory(tmp);

            if (File.Exists(path))
            {
                if (overwrite)
                    File.Delete(path);
                else
                    throw new Exception(
                        $"Output file '{path}' already exists.\nSet the overwrite flag to true to overwrite");
            }

            var zipFile = ZipFile.Open(path, ZipArchiveMode.Create);
            var hashMap = new Dictionary<string, string>();

            foreach (var bundleFile in Bundles)
            {
                var stream = new StreamWriter(Path.Join(tmp, bundleFile.Hash));
                stream.Write(bundleFile.Content);
                stream.Close();

                zipFile.CreateEntryFromFile(Path.Join(tmp, bundleFile.Hash), bundleFile.Hash);
                hashMap[bundleFile.AccessName] = bundleFile.Hash;
            }

            var hashMapStream = new StreamWriter(Path.Join(tmp, "hashmap"));
            foreach (var valuePair in hashMap) hashMapStream.Write($"{valuePair.Key}={valuePair.Value}\n");

            hashMapStream.Close();

            zipFile.CreateEntryFromFile(Path.Join(tmp, "hashmap"), "hashmap");
            zipFile.Dispose();

            Directory.Delete(tmp, true);
        }

        public static List<BundleEntry> ReadBundle(string path)
        {
            var zip = ZipFile.Open(path, ZipArchiveMode.Read);
            var hashMap = HashMap.GetHashMap(zip);

            var files = new List<BundleEntry>();

            foreach (var access in hashMap.Keys())
            {
                var content = ReadContent(hashMap[access], zip);

                files.Add(new BundleEntry
                {
                    AccessName = access,
                    Hash = hashMap[access],
                    Content = content,
                    RawNode = Json.BuildByString(content)
                });
            }

            return files;
        }

        private static string ReadContent(string hash, ZipArchive zip)
        {
            var entry = zip.GetEntry(hash);
            if (entry != null)
            {
                var stream = entry.Open();
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            throw new Exception($"Cannot find {hash} in ila");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using IllusionScript.SDK;
using IllusionScript.SDK.Bundler;
using IllusionScript.SDK.Nodes;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;

namespace IllusionScript
{
    internal class Program
    {
        public static InIFile Config;

        private static void LoadExtension()
        {
            var root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var extensions = new List<string>();

            foreach (var extension in Config.Read("Extensions", "extension"))
                extensions.Add(Path.Join(root, extension));

            PluginLoader.Assemblies = extensions;
            PluginLoader.LoadPlugins();
        }

        private static void SetupConfig()
        {
            var fileImport = Config.Read("Config", "allowFileImport");
            Constants.Config.FileImport = fileImport.Count > 0 && fileImport[0] == "true";
            var fileExport = Config.Read("Config", "allowFileExport");
            Constants.Config.FileExport = fileExport.Count > 0 && fileExport[0] == "true";
        }

        public static void Main(string[] args)
        {
            Config = new InIFile(Path.Join(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ils.ini"));
            LoadExtension();
            SetupConfig();

            var programArgs = new List<string>();
            var interpreterArgs = new List<string>();
            var isInterpreterArgs = true;

            for (var i = 0; i < args.Length; i++)
            {
                var s = args[i];

                if (isInterpreterArgs)
                {
                    if (s == "-f")
                    {
                        var arg = s;
                        interpreterArgs.Add(arg);
                        i++;
                        interpreterArgs.Add(args[i]);
                        isInterpreterArgs = false;
                    }
                    else
                    {
                        interpreterArgs.Add(s);
                    }
                }
                else
                {
                    programArgs.Add(s);
                }
            }

            if (interpreterArgs.Contains("-d"))
            {
                for (var i = 0; i < PluginLoader.Assemblies.Count; i++)
                {
                    Console.WriteLine(PluginLoader.Assemblies[i].Replace("/", "\\"));
                    var plugin = PluginLoader.Plugins[i];
                    var fake = new SymbolTable();
                    Console.WriteLine($"[{plugin.Name}]");
                    plugin.Load(fake);
                    foreach (var key in fake.GetKeys()) Console.WriteLine($"> {key}");

                    Console.WriteLine(" ");
                }

                Console.WriteLine(" ");
            }

            Interpreter.Argv = programArgs;
            for (var i = 0; i < interpreterArgs.Count; i++)
            {
                var arg = interpreterArgs[i];

                if (arg == "-f")
                {
                    i++;
                    var file = interpreterArgs[i];

                    string data;
                    FileInfo fileInfo;

                    if (file.StartsWith("."))
                    {
                        data = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), file));
                        fileInfo = new FileInfo(Path.Join(Directory.GetCurrentDirectory(), file));
                    }
                    else
                    {
                        data = File.ReadAllText(file);
                        fileInfo = new FileInfo(file);
                    }

                    Execute(data, fileInfo.Name, fileInfo.DirectoryName, interpreterArgs.Contains("-r"), true);
                    Environment.Exit(0);
                }
                else if (arg == "-c")
                {
                    string path;
                    i++;

                    if (arg.StartsWith("."))
                        path = Path.Join(Directory.GetCurrentDirectory(), interpreterArgs[i]);
                    else
                        path = interpreterArgs[i];

                    Bundle(path);
                    Environment.Exit(0);
                }
            }

            Shell();
        }

        private static List<string> ReadDir(string path, bool subDirs)
        {
            var entries = new List<string>();

            if (Directory.Exists(path))
            {
                foreach (var x in Directory.GetFiles(path))
                {
                    var entry = new FileInfo(x).Name;
                    if (entry.EndsWith(".ils")) entries.Add(Path.Join(path, entry));
                }

                if (subDirs)
                    foreach (var x in Directory.GetDirectories(path))
                    {
                        var entry = new DirectoryInfo(x).Name;
                        entries.AddRange(ReadDir(Path.Join(path, entry), true));
                    }
            }
            else
            {
                throw new Exception($"'{path}' do not exists or is not a directory");
            }

            return entries;
        }

        private static void Bundle(string conf)
        {
            var reader = XmlReader.Create(conf);
            var files = new List<string>();
            var overwrite = false;
            string target = default;
            string name = default;

            while (reader.Read())
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "folder":
                            var sub = reader.GetAttribute("subDirs") == "true";
                            reader.Read();
                            files.AddRange(ReadDir(Path.Join(Directory.GetCurrentDirectory(), reader.Value), sub));
                            break;
                        case "file":
                            reader.Read();
                            var path = reader.Value.StartsWith(".")
                                ? Path.Join(Directory.GetCurrentDirectory(), reader.Value)
                                : reader.Value;
                            if (File.Exists(path) && path.EndsWith(".ils"))
                                files.Add(path);
                            else
                                throw new Exception($"File '{path}' not found or is not a ils file");

                            break;
                        case "target":
                            reader.Read();
                            target = Path.IsPathRooted(reader.Value)
                                ? reader.Value
                                : Path.Join(Directory.GetCurrentDirectory(), reader.Value);
                            break;
                        case "name":
                            reader.Read();
                            name = reader.Value;
                            if (!name.EndsWith(".ila")) name += ".ila";

                            break;
                        case "overwrite":
                            reader.Read();
                            overwrite = reader.Value == "true";
                            break;
                    }

            if (name == default)
                throw new Exception("No name in the config file is given");
            if (target == default)
                throw new Exception("No output dir is in the config file declared");
            if (files.Count == 0) throw new Exception("Cannot bundle zero files");

            Console.WriteLine($"Output: {target}");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine("\nFiles:");
            foreach (var s in files) Console.WriteLine("> " + s.Replace("/", "\\"));

            Console.WriteLine("\n");

            var converter = new Converter();

            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];

                Console.WriteLine($"[{i + 1}|{files.Count}] Link '{file.Replace("/", "\\")}'");
                var content = File.ReadAllText(file);
                var info = new FileInfo(file);
                var lexer = new Lexer(content, info.Name, info.DirectoryName);
                var lexerRes = lexer.MakeTokens();

                if (lexerRes.Item1 != default(Error))
                {
                    Console.WriteLine(lexerRes.Item1.ToString());
                    Environment.Exit(1);
                }

                var parser = new Parser(lexerRes.Item2);
                var parserRes = parser.Parse();
                if (parserRes.Error != default(Error))
                {
                    Console.WriteLine(parserRes.Error.ToString());
                    Environment.Exit(1);
                }

                converter.Bundle((ListNode)parserRes.Node);
            }

            var output = Path.Join(target, name);

            if (!Directory.Exists(target)) Directory.CreateDirectory(target);

            Console.WriteLine("\nWrite down");
            converter.WriteDown(output, overwrite);
        }

        private static void Execute(string data, string fileName, string filepath, bool showResult = true,
            bool main = false)
        {
            var context = new Context("<program>")
            {
                SymbolTable = SymbolTable.GlobalSymbols
            };

            var res = Executor.Run(data, fileName, filepath, context,
                main);
            if (res.Item1 != default(Error))
            {
                Console.Write(res.Item1 + Constants.EOL);
            }
            else
            {
                if (res.Item2 != default(Value) && showResult)
                {
                    if (res.Item2.GetType() == typeof(ListValue))
                    {
                        var list = (ListValue)res.Item2;

                        Console.Write(Constants.EOL);
                        if (list.Elements.Count == 1)
                            Console.Write(list.Elements[0].__repr__(0));
                        else
                            Console.Write(list.__repr__(0));
                    }
                    else
                    {
                        Console.Write(res.Item1.ToString());
                    }
                }
            }

            Console.Write(Constants.EOL);
        }

        private static void Shell()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input.Trim() == "") continue;

                Execute(input, "<stdin>", Directory.GetCurrentDirectory());
            }
        }
    }
}
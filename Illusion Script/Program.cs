using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using IllusionScript.SDK;
using IllusionScript.SDK.Nodes;
using IllusionScript.SDK.Plugin;
using IllusionScript.SDK.Values;
using IllusionScript.SDK.Bundler;

namespace IllusionScript
{
    class Program
    {
        public static InIFile Config;

        private static void LoadExtension()
        {
            string root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            List<string> extensions = new List<string>();

            foreach (string extension in Config.Read("Extensions", "extension"))
            {
                extensions.Add(Path.Join(root, extension));
            }

            PluginLoader.Assemblies = extensions;
            PluginLoader.LoadPlugins();
        }

        private static void SetupConfig()
        {
            List<string> fileImport = Config.Read("Config", "allowFileImport");
            Constants.Config.FileImport = fileImport.Count > 0 && fileImport[0] == "true";
            List<string> fileExport = Config.Read("Config", "allowFileExport");
            Constants.Config.FileExport = fileExport.Count > 0 && fileExport[0] == "true";
        }

        public static void Main(string[] args)
        {
            Config = new InIFile(Path.Join(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ils.ini"));
            LoadExtension();
            SetupConfig();

            List<string> programArgs = new List<string>();
            List<string> interpreterArgs = new List<string>();
            bool isInterpreterArgs = true;

            for (int i = 0; i < args.Length; i++)
            {
                string s = args[i];

                if (isInterpreterArgs)
                {
                    if (s == "-f")
                    {
                        string arg = s;
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
                for (int i = 0; i < PluginLoader.Assemblies.Count; i++)
                {
                    Console.WriteLine(PluginLoader.Assemblies[i].Replace("/", "\\"));
                    IModule plugin = PluginLoader.Plugins[i];
                    SymbolTable fake = new SymbolTable();
                    Console.WriteLine($"[{plugin.Name}]");
                    plugin.Load(fake);
                    foreach (string key in fake.GetKeys())
                    {
                        Console.WriteLine($"> {key}");
                    }

                    Console.WriteLine(" ");
                }

                Console.WriteLine(" ");
            }

            Interpreter.Argv = programArgs;
            for (int i = 0; i < interpreterArgs.Count; i++)
            {
                string arg = interpreterArgs[i];

                if (arg == "-f")
                {
                    i++;
                    string file = interpreterArgs[i];

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
                    {
                        path = Path.Join(Directory.GetCurrentDirectory(), interpreterArgs[i]);
                    }
                    else
                    {
                        path = interpreterArgs[i];
                    }

                    Bundle(path);
                    Environment.Exit(0);
                }
            }

            Shell();
        }

        private static List<string> ReadDir(string path, bool subDirs)
        {
            List<string> entries = new List<string>();

            if (Directory.Exists(path))
            {
                foreach (string x in Directory.GetFiles(path))
                {
                    string entry = new FileInfo(x).Name;
                    if (entry.EndsWith(".ils"))
                    {
                        entries.Add(Path.Join(path, entry));
                    }
                }

                if (subDirs)
                {
                    foreach (string x in Directory.GetDirectories(path))
                    {
                        string entry = new DirectoryInfo(x).Name;
                        entries.AddRange(ReadDir(Path.Join(path, entry), true));
                    }
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
            XmlReader reader = XmlReader.Create(conf);
            List<string> files = new List<string>();
            bool overwrite = false;
            string target = default;
            string name = default;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "folder":
                            bool sub = reader.GetAttribute("subDirs") == "true";
                            reader.Read();
                            files.AddRange(ReadDir(Path.Join(Directory.GetCurrentDirectory(), reader.Value), sub));
                            break;
                        case "file":
                            reader.Read();
                            string path = reader.Value.StartsWith(".")
                                ? Path.Join(Directory.GetCurrentDirectory(), reader.Value)
                                : reader.Value;
                            if (File.Exists(path) && path.EndsWith(".ils"))
                            {
                                files.Add(path);
                            }
                            else
                            {
                                throw new Exception($"File '{path}' not found or is not a ils file");
                            }

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
                            if (!name.EndsWith(".ila"))
                            {
                                name += ".ila";
                            }

                            break;
                        case "overwrite":
                            reader.Read();
                            overwrite = reader.Value == "true";
                            break;
                    }
                }
            }

            if (name == default)
            {
                throw new Exception("No name in the config file is given");
            }
            else if (target == default)
            {
                throw new Exception("No output dir is in the config file declared");
            }
            else if (files.Count == 0)
            {
                throw new Exception("Cannot bundle zero files");
            }

            Console.WriteLine($"Output: {target}");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine("\nFiles:");
            foreach (string s in files)
            {
                Console.WriteLine("> " + s.Replace("/", "\\"));
            }

            Console.WriteLine("\n\n");

            Converter converter = new Converter();

            foreach (string file in files)
            {
                Console.WriteLine($"Link '{file.Replace("/", "\\")}'");
                string content = File.ReadAllText(file);
                FileInfo info = new FileInfo(file);
                Lexer lexer = new Lexer(content, info.Name, info.DirectoryName);
                Tuple<Error, List<Token>> lexerRes = lexer.MakeTokens();

                if (lexerRes.Item1 != default(Error))
                {
                    Console.WriteLine(lexerRes.Item1.ToString());
                    Environment.Exit(1);
                }

                Parser parser = new Parser(lexerRes.Item2);
                ParserResult parserRes = parser.Parse();
                if (parserRes.Error != default(Error))
                {
                    Console.WriteLine(parserRes.Error.ToString());
                    Environment.Exit(1);
                }

                converter.Bundle((ListNode)parserRes.Node);
            }

            string output = Path.Join(target, name);

            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            Console.WriteLine("\nWrite down");
            converter.WriteDown(output, overwrite);
        }

        private static void Execute(string data, string fileName, string filepath, bool showResult = true,
            bool main = false)
        {
            Context context = new Context("<program>")
            {
                SymbolTable = new SymbolTable()
            };

            Tuple<Error, Value, Dictionary<string, Value>> res = Executor.Run(data, fileName, filepath, context,
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
                        ListValue list = (ListValue)res.Item2;

                        Console.Write(Constants.EOL);
                        if (list.Elements.Count == 1)
                        {
                            Console.Write(list.Elements[0].__repr__(0));
                        }
                        else
                        {
                            Console.Write(list.__repr__(0));
                        }
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
                string input = Console.ReadLine();
                if (input.Trim() == "")
                {
                    continue;
                }

                Execute(input, "<stdin>", Directory.GetCurrentDirectory());
            }
        }
    }
}
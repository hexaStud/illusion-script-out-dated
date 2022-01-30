using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
                        i++;
                        arg += "=" + args[i];
                        interpreterArgs.Add(arg);
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
                    Console.WriteLine(PluginLoader.Assemblies[i]);
                    IModule plugin = PluginLoader.Plugins[i];
                    SymbolTable fake = new SymbolTable();
                    plugin.Load(fake);
                    foreach (string key in fake.GetKeys())
                    {
                        Console.WriteLine($"  > {key}");
                    }

                    Console.WriteLine(" ");
                }

                Console.WriteLine(" ");
            }

            Interpreter.Argv = programArgs;

            foreach (string arg in interpreterArgs)
            {
                if (arg.StartsWith("-f="))
                {
                    string file = arg.Split('=')[1];

                    string data = "";
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
            }

            Shell();
        }

        private static void Execute(string data, string fileName, string filepath, bool showResult = true,
            bool main = false)
        {
            Context context = new Context("<program>");
            context.SymbolTable = SymbolTable.GlobalSymbols;

            Lexer lexer = new Lexer(data, fileName, filepath);
            Tuple<Error, List<Token>> testRes = lexer.MakeTokens();
            Parser parser = new Parser(testRes.Item2);
            ParserResult parserResult = parser.Parse();
            Converter bundler = new Converter();
            bundler.Bundle((ListNode)parserResult.Node);
            bundler.WriteDown(Path.Join(Directory.GetCurrentDirectory(), "test.ila"));


            return;
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
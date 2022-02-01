using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IllusionScript.SDK.Plugin
{
    public static class PluginLoader
    {
        public static List<string> Assemblies = new List<string>();
        public static List<IModule> Plugins = new List<IModule>();

        public static void LoadPlugins()
        {
            Plugins = new List<IModule>();
            foreach (string assembly in Assemblies)
            {
                if (File.Exists(assembly) && assembly.EndsWith(".dll"))
                {
                    Assembly.LoadFile(assembly);
                }
            }

            Type interfaceType = typeof(IModule);
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToArray();

            foreach (Type type in types)
            {
                Plugins.Add((IModule)Activator.CreateInstance(type));
            }
        }

        public static bool Exists(string name)
        {
            foreach (IModule plugin in Plugins)
            {
                if (plugin.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Bind(string name, SymbolTable symbolTable)
        {
            foreach (IModule plugin in Plugins)
            {
                if (plugin.Name != name)
                {
                    continue;
                }
                plugin.Load(symbolTable);
                break;
            }
        }
    }
}
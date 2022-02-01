using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IllusionScript.SDK.Plugin
{
    public static class PluginLoader
    {
        public static List<string> Assemblies = new();
        public static List<IModule> Plugins = new();

        public static void LoadPlugins()
        {
            Plugins = new List<IModule>();
            foreach (var assembly in Assemblies)
                if (File.Exists(assembly) && assembly.EndsWith(".dll"))
                    Assembly.LoadFile(assembly);

            var interfaceType = typeof(IModule);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToArray();

            foreach (var type in types) Plugins.Add((IModule)Activator.CreateInstance(type));
        }

        public static bool Exists(string name)
        {
            foreach (var plugin in Plugins)
                if (plugin.Name == name)
                    return true;

            return false;
        }

        public static void Bind(string name, SymbolTable symbolTable)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.Name != name) continue;
                plugin.Load(symbolTable);
                break;
            }
        }
    }
}
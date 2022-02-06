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
        public static List<PluginItem> Plugins = new List<PluginItem>();

        public static void LoadPlugins()
        {
            Plugins = new List<PluginItem>();
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
                Plugins.Add(new PluginItem()
                {
                    Module = (IModule)Activator.CreateInstance(type),
                    Assembly = type.Assembly.Location
                });
            }
        }

        public static bool Exists(string name)
        {
            foreach (PluginItem plugin in Plugins)
            {
                if (plugin.Module.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Bind(string name, SymbolTable symbolTable)
        {
            foreach (PluginItem plugin in Plugins)
            {
                if (plugin.Module.Name != name)
                {
                    continue;
                }

                plugin.Module.Load(symbolTable);
                break;
            }
        }

        public static List<PluginItem> GetAllPlugins(string path)
        {
            path = SDK.Extensions.Path.Join(path);
            List<PluginItem> pluginItems = new List<PluginItem>();
            foreach (PluginItem pluginItem in Plugins)
            {
                if (pluginItem.Assembly == path)
                {
                    pluginItems.Add(pluginItem);
                }
            }

            return pluginItems;
        }
    }
}
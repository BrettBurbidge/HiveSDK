using Node.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Shell
{
    public class PluginLoader
    {
        public static List<IPlugin> Plugins { get; private set; } = new List<IPlugin>();


        /// <summary>
        /// Reads from the local plugin folder nad rebuilds the plugin file. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task LoadPlugins(ILogger logger, string path, WorkEventHandler workEventHandler )
        {
            logger.LogInformation($"Loading plugins using: {path}");

            string[] pluginDirectories = Directory.GetDirectories(path);

            foreach (string pluginDirectory in pluginDirectories)
            {
                await LoadSinglePluginFromDirectory(logger, pluginDirectory, workEventHandler);
            }
        }

        public static async Task LoadSinglePluginFromDirectory(ILogger logger, string path, WorkEventHandler workEventHandler)
        {
            string[] pluginFiles = Directory.GetFiles(path, "*.dll");

            foreach (string dllPath in pluginFiles)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(dllPath);
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface)
                            {
                                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                                Plugins.Add(plugin);
                                plugin.Initialize();
                                logger.LogInformation($"Loaded plugin: {plugin.Name}");
                            }
                        }

                        workEventHandler.SetPlugins(Plugins);   
                    } catch (Exception ex)
                    {
                        logger.LogError($"Error loading plugin from {dllPath}: {ex.Message}");
                    }
                });
            }
        }
    }
}

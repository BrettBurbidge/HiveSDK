using System.Reflection;
using HiveServer.SDK;

namespace HiveServer.Services
{
    //Not finished with this. We want this to load the Server side plugins
    public class PluginManager
    {
        public List<IHiveServerPlugin> Plugins { get; private set; } = new List<IHiveServerPlugin>();

        public async Task LoadPlugins(string path)
        {
            foreach (string dllPath in Directory.GetFiles(path, "*.dll"))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(dllPath);
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (typeof(IHiveServerPlugin).IsAssignableFrom(type) && !type.IsInterface)
                            {
                                IHiveServerPlugin plugin = (IHiveServerPlugin)Activator.CreateInstance(type);
                                Plugins.Add(plugin);
                                plugin.Initialize();
                                Console.WriteLine($"Loaded plugin: {plugin.Name}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading plugin from {dllPath}: {ex.Message}");
                    }
                });
            }
        }

        public void NotifyPlugins(object sender, EventArgs e)
        {
            foreach (var plugin in Plugins)
            {
                plugin.OnWorkToDo(sender, e);
            }
        }
    }
}

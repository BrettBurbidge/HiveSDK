using Node.SDK;
using System.Reflection;


namespace NodeShell
{
    public static class StaticPluginManager
    {
        public static List<IPlugin> Plugins { get; private set; } = new List<IPlugin>();

        public static async Task LoadPlugins(string path)
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
                            if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface)
                            {
                                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
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

        public static void NotifyPlugins(object sender, WorkEventArgs e)
        {
            foreach (var plugin in Plugins)
            {
                try
                {
                    plugin.OnWorkToDo(sender, e);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Notify Plugins failed...{ex.Message}");
                }
                
            }
        }
    }
}

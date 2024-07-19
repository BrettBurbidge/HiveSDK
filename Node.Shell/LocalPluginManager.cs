using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Node.Shell
{
        public class LocalPluginManager
    {
        public class Plugin
        {
            public string PluginName { get; set; }
            public string Version { get; set; }
            public DateTime LastUpdate { get; set; }
        }

        private List<Plugin> _plugins;
        private readonly string _filePath;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public LocalPluginManager(string filePath)
        {
            _filePath = filePath;
            _plugins = new List<Plugin>();
        }

        public async Task LoadFromFileAsync()
        {
            if (File.Exists(_filePath))
            {
                using FileStream stream = File.OpenRead(_filePath);
                _plugins = await JsonSerializer.DeserializeAsync<List<Plugin>>(stream) ?? new List<Plugin>();
            }
        }

        public async Task SaveToFileAsync()
        {
            using FileStream stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, _plugins, _jsonOptions);
        }

        public void AddPlugin(string name, string version)
        {
            _plugins.Add(new Plugin
            {
                PluginName = name,
                Version = version,
                LastUpdate = DateTime.UtcNow
            });
        }

        public void UpdatePlugin(string name, string newVersion)
        {
            Plugin plugin = _plugins.Find(p => p.PluginName == name);
            if (plugin != null)
            {
                plugin.Version = newVersion;
                plugin.LastUpdate = DateTime.UtcNow;
            }
        }

        public void RemovePlugin(string name)
        {
            _plugins.RemoveAll(p => p.PluginName == name);
        }

        public IEnumerable<Plugin> GetAllPlugins()
        {
            return _plugins;
        }

        public Plugin GetPlugin(string name)
        {
            return _plugins.Find(p => p.PluginName == name);
        }
    }

}
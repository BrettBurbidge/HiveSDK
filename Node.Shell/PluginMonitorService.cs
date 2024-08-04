
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Node.SDK;

namespace Node.Shell
{

    /// <summary>
    /// Watches the local plugin directory of this plugin for new folders. When a new folder is found it calls over to the local plugin manager to handle initialization. 
    /// </summary>
    public class PluginMonitorService
    {
        private readonly ILogger _logger;
        private readonly string _directoryToWatch;
        private FileSystemWatcher _watcher;
        private readonly WorkEventHandler _workEventHandler;


        public PluginMonitorService(ILogger logger, string directoryToWatch, WorkEventHandler workEventHandler)
        {
            _logger = logger;
            _directoryToWatch = directoryToWatch;
            _workEventHandler = workEventHandler;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("File Processing Service is starting.");

            _watcher = new FileSystemWatcher(_directoryToWatch)
            {
                NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size
            };

            _watcher.Created += OnFileCreated;
            _watcher.Error += OnWatcherError;

            _watcher.EnableRaisingEvents = true;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _watcher.EnableRaisingEvents = false;
            _logger.LogInformation("File Processing Service is stopping.");
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            //TODO: Check if this plugin is already loaded...
            string fileType = File.Exists(e.FullPath) ? "file" : "folder";
            Console.WriteLine($"Created: {e.FullPath} - Type: {fileType}");
            _logger.LogInformation($"New Plugin detected: {e.Name} at {e.FullPath} - Type: {fileType}");

            // Add your custom logic here
            if (fileType == "file")
            {
                // Handle new file
            } else
            {
                ProcessFile(e.FullPath);
            }
        }

        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            _logger.LogError($"File system watcher error: {e.GetException()}");
        }

        private async Task ProcessFile(string filePath)
        {
            try
            {

               await PluginLoader.LoadSinglePluginFromDirectory(_logger, filePath, _workEventHandler);
            } catch (Exception ex)
            {
                _logger.LogError($"Error processing file {filePath}: {ex.Message}");
            }
        }
    }
}
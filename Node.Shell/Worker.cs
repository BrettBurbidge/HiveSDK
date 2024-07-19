using System.ComponentModel;

namespace Node.Shell;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly PluginMonitorService _pluginMonitorService;
    private readonly string _pluginPath;
    private readonly ActionEventHandler _actionEventHandler;
    private readonly WorkEventHandler _workEventHandler;
    private readonly WorkMonitor _workMonitor;
    private readonly ConsoleWatcherService _consoleWatcher;
    private readonly IHostApplicationLifetime _appLifetime;


    public Worker(ILogger<Worker> logger, ConsoleWatcherService consoleWatcher, IHostApplicationLifetime appLifetime)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consoleWatcher = consoleWatcher;
        _appLifetime = appLifetime;
        _pluginPath = GetPluginLocalPluginDirectory();
        _actionEventHandler = new ActionEventHandler();
        _workEventHandler = new WorkEventHandler(logger, _actionEventHandler);
        _pluginMonitorService = new PluginMonitorService(logger, _pluginPath, _workEventHandler);
        _workMonitor = new WorkMonitor(logger, "https://localhost:7230", _actionEventHandler);

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //Wait for local webserver to start :-}
        await Task.Delay(6000);

        _consoleWatcher.CommandDetected += OnCommandDetected;

        List<Task> tasks = new List<Task>
        {
            Task.Run(() => _consoleWatcher.StartAsync(stoppingToken)),
            Task.Run(() => _pluginMonitorService.StartAsync(stoppingToken)),
            Task.Run(() => _workMonitor.RunAsync(stoppingToken)),
            Task.Run(() => _workEventHandler.HandleEventsAsync(stoppingToken)),
            Task.Run(() => _workEventHandler.HandleEventsAsync(stoppingToken)),


        };
        //When the Plugin monitor sees a new plugin just add the new plugin to this list and keep going. 
        await PluginLoader.LoadPlugins(_logger, _pluginPath, _workEventHandler);

        try
        {
            await Task.WhenAll(tasks);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running tasks.");
        }
    }

    private void OnCommandDetected(object sender, string command)
    {
        _logger.LogInformation($"Command received: {command}");

        switch (command.ToLower())
        {
            case "stop":
                _logger.LogInformation("Stop command received. Initiating shutdown...");
                _appLifetime.StopApplication();
                break;
            case "status":
                Console.WriteLine("Status: Running");
                break;
            case "help":
                Console.WriteLine("Available commands: stop, status, help");
                break;
            default:
                Console.WriteLine($"Unknown command: {command}");
                break;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker is stopping.");
        await base.StopAsync(cancellationToken);
    }

    private string GetPluginLocalPluginDirectory()
    {
        var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "plugins");

        try
        {
            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException($"Pluin Directory is not configured. Node.Shell is expecting a plugin folder here: {path}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        } catch (Exception ex)
        {
            _logger.LogError($"Something went wrong configuring the Node Shell plugin directory: {ex.Message}");
            throw;
        }
    }
}

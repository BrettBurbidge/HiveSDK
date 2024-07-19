using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Node.Shell
{
    /// <summary>
    /// Watches for console input and lets the main thread (Worker.cs) know that an command was provided. All controls are handled in the Worker.cs class. 
    /// </summary>
    public class ConsoleWatcherService : BackgroundService
    {
        private readonly ILogger<ConsoleWatcherService> _logger;
        private readonly string[] _watchWords;

        public event EventHandler<string> CommandDetected;

        public ConsoleWatcherService(ILogger<ConsoleWatcherService> logger, string[] watchWords)
        {
            _logger = logger;
            _watchWords = watchWords;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Console Watcher Service is starting.");

            try
            {
                await WatchConsoleInputAsync(stoppingToken);
            } catch (OperationCanceledException)
            {
                _logger.LogInformation("Console Watcher Service is stopping.");
            } catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Console Watcher Service.");
            }
        }

        private async Task WatchConsoleInputAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.Write("> ");
                var input = await ReadLineAsync(stoppingToken);

                if (!string.IsNullOrEmpty(input))
                {
                    foreach (var word in _watchWords)
                    {
                        if (input.Equals(word, StringComparison.OrdinalIgnoreCase))
                        {
                            _logger.LogInformation($"Command detected: {word}");
                            OnCommandDetected(word);
                        }
                    }
                }
            }
        }

        private async Task<string> ReadLineAsync(CancellationToken cancellationToken)
        {
            var input = new System.Text.StringBuilder();
            var readTask = Task.Run(() =>
            {
                while (true)
                {
                    var keyInfo = Console.ReadKey(intercept: true);

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    } else if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input.Remove(input.Length - 1, 1);
                        Console.Write("\b \b");
                    } else
                    {
                        input.Append(keyInfo.KeyChar);
                        Console.Write(keyInfo.KeyChar);
                    }
                }
            }, cancellationToken);

            await readTask;
            return input.ToString();
        }

        protected virtual void OnCommandDetected(string command)
        {
            CommandDetected?.Invoke(this, command);
        }
    }

}
using Node.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Node.Shell
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ConsoleWatcherService>(provider =>
                        new ConsoleWatcherService(
                            provider.GetRequiredService<ILogger<ConsoleWatcherService>>(),
                            new string[] { "stop", "status", "help" }
                        )
                    );
                    services.AddHostedService<Worker>();
                });
    }
}
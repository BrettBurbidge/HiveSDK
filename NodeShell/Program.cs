using System.Reflection;
using Node.SDK;

namespace NodeShell
{
    internal class Program
    {
        private static WorkMonitor _workMonitor = new WorkMonitor();

        static async Task Main(string[] args)
        {
            Console.WriteLine($"Starting Node.Shell");
#if DEBUG
            Thread.Sleep(6000);  // Wait for Webserver to Start when debugging. 
#endif
            var program = new Program();
            await program.InitNodeShell();

            // Keep the program running
            //while (true)
            //{
            //    Thread.Sleep(1000);  // Sleep for a short time to prevent busy waiting
            //}
            // Keep the program running
            Thread.Sleep(Timeout.Infinite);
        }

        private async Task InitNodeShell()
        {
            Console.WriteLine($"Loading Plugins...");
            Task loadPluginsTask = StaticPluginManager.LoadPlugins("C:\\Users\\brett\\source\\repos\\Nerd\\IncentiveEngine\\NodeShell\\GreenPowerCheck\\bin\\Debug\\net7.0");

            Console.WriteLine($"Register work handler...");
            // Subscribe to the WorkToDo event
            _workMonitor.WorkToDo += OnWorkToDo;

            Console.WriteLine($"Start looking for  work...");
            //Start looking for work
            _workMonitor.StartWorkMonitor();

            await loadPluginsTask;
        }

        private static void OnWorkToDo(object sender, WorkEventArgs e)
        {
            StaticPluginManager.NotifyPlugins(sender, e);
        }
    }
}

